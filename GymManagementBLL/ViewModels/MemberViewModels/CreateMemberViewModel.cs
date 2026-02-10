using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
	public class CreateMemberViewModel
	{
		[Required(ErrorMessage ="Profile Photo Is Required")]
		[Display(Name="Profile Photo")]
		public IFormFile PhotoFile { get; set; } = null!;

		[Required(ErrorMessage ="Name Is Required")]
		[StringLength(50,MinimumLength =2,ErrorMessage ="Name Must be Between 2 and 50")]
		[RegularExpression(@"^[a-zA-Z]+(\s[a-zA-z]+)*$")]
		public string Name { get; set; } = null!;
		[Required(ErrorMessage ="Email Is Required")]
		[StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 and 100")]
		[EmailAddress(ErrorMessage ="Invalid Email Format")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }=null!;
		[Required(ErrorMessage ="Phone Is Required")]
		[Phone(ErrorMessage ="Invalid Phone Format")]
		[RegularExpression(@"^(010||011||012||015)\d{8}$")]
		public string Phone { get; set; }=null !;
		[Required(ErrorMessage = "Gender Is Required")]
		public Gender Gender { get; set; } 
		[Required(ErrorMessage ="Date Of Birth Is Required")]
		[DataType(DataType.Date)] 
		public DateOnly DateOfBirth { get; set; }
		[Required(ErrorMessage ="Building Number Is Required")]
		[Range(1,9000,ErrorMessage ="Building Number Must Be Between 1 and 9000")]
		public int BuildingNumber { get; set; }
		[Required(ErrorMessage ="Street Is Required")]
		[StringLength(30,MinimumLength =2,ErrorMessage ="Street Must be Between 2 and 30")]
		public string Street { get; set; } = null!;
		[Required(ErrorMessage = "City Is Required")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30")]
		[RegularExpression(@"^[a-zA-Z\s]+$")]
		public string City { get; set; } = null!;
		[Required(ErrorMessage ="Health Record Is Required")]
		public HealthRecordViewModel HealthRecordViewModel { get; set; }


	}
}
