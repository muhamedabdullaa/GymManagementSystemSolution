using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.DataSeed
{
	public static class IDentityDbContextSeeding
	{
		public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			try
			{
				var HasUsers = userManager.Users.Any();
				var HasRoles = roleManager.Roles.Any();
				if (HasUsers && HasRoles) return false;
				if (!HasRoles)
				{
					var Roles = new List<IdentityRole>()
					{
						new(){Name="SuperAdmin"},
						new(){Name="Admin"}

					};
					foreach (var Role in Roles)
					{
						if(!roleManager.RoleExistsAsync(Role.Name!).Result)
						roleManager.CreateAsync(Role).Wait();
					}
				}
				if (!HasUsers)
				{
					var MainAdmin = new ApplicationUser()
					{
						FirstName = "Muhamed",
						LastName = "Abdallah",
						UserName = "MohamedAbdallah",
						Email = "MohamedAbdallah@gmail.com",
						PhoneNumber = "01000812748"
					};
					userManager.CreateAsync(MainAdmin ,"P@ssw0rd").Wait();
					userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

					var Admin = new ApplicationUser()
					{
						FirstName = "Ahmed",
						LastName = "Abdallah",
						UserName = "AhmedAbdallah",
						Email = "AhmedAbdallah@gmail.com",
						PhoneNumber = "01000812748"
					};
					userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
					userManager.AddToRoleAsync(Admin, "Admin").Wait();

					
				}
				return true;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"Seed Failed : {ex}");
				return false;
			}
		}
	}
}
