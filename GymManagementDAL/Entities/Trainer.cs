using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	public class Trainer : GymUser
	{
		// HireDate = Created At - BaseEntity
		public Specialists Specialist { get; set; }

		public ICollection<Session> Sessions { get; set; } = null!;

	}
}
