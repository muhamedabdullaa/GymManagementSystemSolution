using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
	public class SessionRepository : GenericRepository<Session>, ISessionRepository
	{
		private readonly GymDbContext dbContext;

		public SessionRepository(GymDbContext dbContext):base(dbContext)
		{
			this.dbContext = dbContext;
		}
		public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
		{
			return dbContext.Sessions.Include(X => X.Category)
								.Include(X => X.Trainer)
								.ToList();
		}

		public int GetCountOfBookedSlots(int SessionId)
		{
			return dbContext.MemberSessions.Count(X=>X.SessionId == SessionId);
		}

		public Session? GetSessionWithTrainerAndCategoryById(int SessionId)
		{
			return dbContext.Sessions.Include(X => X.Category)
								.Include(X => X.Trainer)
								.FirstOrDefault(X=>X.Id == SessionId);
		}
	}
}
