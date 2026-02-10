using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
	public class UpdateSessionViewModel
	{
		[Required(ErrorMessage ="Description Is Required")]
		[StringLength(500,MinimumLength =5,ErrorMessage ="Description Must Be Between 5 and 500 Char")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Start Date Is Required")]
		[Display(Name ="Start Date & Time")]
		public DateTime StartDate { get; set; }
		[Required(ErrorMessage = "End Date Is Required")]
		[Display(Name = "End Date & Time")]
		public DateTime EndDate { get; set; }
		[Required(ErrorMessage = "Trainer Is Required")]
		[Display(Name = "Trainer")]
		public int TrainerId { get; set; }

	}
}
