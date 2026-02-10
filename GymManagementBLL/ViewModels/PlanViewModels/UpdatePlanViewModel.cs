using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
	public class UpdatePlanViewModel
	{
		public string PlanName { get; set; } = null!;
		[Required(ErrorMessage = "Description Is Required")]
		[StringLength(200,MinimumLength =5 , ErrorMessage = "Description Must Be Between 5 And 200")]
		public string Description { get; set; } = null!;
		[Required(ErrorMessage ="Duration Days Is Required")]
		[Range(1,365)]
		public int DurationDays { get; set; }
		[Required(ErrorMessage = "Price Is Required")]
		[Range(0.1, 10000)]
		public decimal Price { get; set; }

	}
}
