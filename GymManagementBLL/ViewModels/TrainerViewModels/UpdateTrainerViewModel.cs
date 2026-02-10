using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
	public class UpdateTrainerViewModel
	{
		[Required(ErrorMessage = "Name Is Required")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 and 50")]
		[RegularExpression(@"^[a-zA-Z]\s+$")]
		public string Name { get; set; } = null!;
		[Required(ErrorMessage = "Email Is Required")]
		[StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 and 100")]
		[EmailAddress(ErrorMessage = "Invalid Email Format")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;
		public Address Address { get; set; }
		[Required(ErrorMessage = "Phone Is Required")]
		[Phone(ErrorMessage = "Invalid Phone Format")]
		[RegularExpression(@"^(010||011||012||015)\d{8}$")]
		public string Phone { get; set; } = null!;
		public Specialists Specialist { get; set; }
	}
}
