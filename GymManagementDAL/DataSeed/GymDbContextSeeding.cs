using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.DataSeed
{
	public static class GymDbContextSeeding
	{
		public static bool SeedData(GymDbContext dbContext)
		{

			try
			{
				var HasPlan = dbContext.Plans.Any();
				var HasCategory = dbContext.Categories.Any();
				if (HasPlan && HasCategory) return false;

				if (!HasPlan)
				{
					var plans = LoadDataFromJsonFile<Plan>("plans.json");
					if (plans.Any())
						dbContext.Plans.AddRange(plans);

				}
				if (!HasCategory)
				{
					var categories = LoadDataFromJsonFile<Category>("categories.json");
					if (categories.Any())
						dbContext.Categories.AddRange(categories);

				}
				return dbContext.SaveChanges() > 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"Seeding Failed : {ex} ");
				return false;
			}
		}

		private static List<T> LoadDataFromJsonFile<T>(string fileName)
		{

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
			if (!File.Exists(filePath)) throw new FileNotFoundException();
			var data = File.ReadAllText(filePath);
			var Options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};
			return JsonSerializer.Deserialize<List<T>>(data, Options) ?? new List<T>();
		}
	}
}
