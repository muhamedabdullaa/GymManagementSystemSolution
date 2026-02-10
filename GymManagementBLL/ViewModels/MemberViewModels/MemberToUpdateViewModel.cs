using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
	public class MemberToUpdateViewModel
	{
		public string Name { get; set; }
		public string? Photo { get; set; }
		public string Email { get; set; } = null!;
		[Required(ErrorMessage = "Phone Is Required")]
		[Phone(ErrorMessage = "Invalid Phone Format")]
		[RegularExpression(@"^(010||011||012||015)\d{8}$")]
		public string Phone { get; set; } = null!;
		[Required(ErrorMessage = "Building Number Is Required")]
		[Range(1, 9000, ErrorMessage = "Building Number Must Be Between 1 and 9000")]
		public int BuildingNumber { get; set; }
		[Required(ErrorMessage = "Street Is Required")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 and 30")]
		public string Street { get; set; } = null!;
		[Required(ErrorMessage = "City Is Required")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30")]
		[RegularExpression(@"^[a-zA-Z]\s+$")]
		public string City { get; set; } = null!;
	}
}
