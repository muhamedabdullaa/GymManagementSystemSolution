using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	public class HealthRecord : BaseEntity
	{
		// Relation 1 To 1 With Member
		public decimal? Height { get; set; } = null!;
		public decimal? Weight { get; set; } = null!;
		public string BloodType { get; set; } = null!;
		public string? Note { get; set; }
	}
}
