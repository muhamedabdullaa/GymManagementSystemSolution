using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	public class AnalyticService : IAnalyticService
	{
		private readonly IUnitOfWork unitOfWork;

		public AnalyticService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		
		public AnalyticsViewModel GetAnalyticsData()
		{
			var sessions = unitOfWork.GetRepository<Session>().GetAll();
			return new AnalyticsViewModel()
			{
				ActiveMembers = unitOfWork.GetRepository<MemberShip>().GetAll(X=>X.Status == "Active").Count(),
				TotalMembers = unitOfWork.GetRepository<Member>().GetAll().Count(),
				TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
				UpComingSessions = sessions.Count(X=>X.StartDate > DateTime.Now),
				OnGoingSessions = sessions.Count(X=>X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now),
				CompletedSessions = sessions.Count(X=>X.EndDate < DateTime.Now)
			};
		}
	}
}
