using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
	public class TrainerViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Email { get; set; }=null!;
		public DateOnly DateOfBirth { get; set; }
		public Address Address { get; set; }
		public string Phone { get; set; }
		public Specialists Specialist { get; set; } 

	}
}
