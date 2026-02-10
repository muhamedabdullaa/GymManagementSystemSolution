using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	public class SessionService : ISessionService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly ISessionRepository sessionRepository;
		private readonly IMapper mapper;

		public SessionService(IUnitOfWork unitOfWork, IMapper _mapper)
		{
			this.unitOfWork = unitOfWork;
			
			mapper = _mapper;
		}
		public IEnumerable<SessionViewModel> GatAllSessions()
		{
			var sessions = unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
			if (!sessions.Any()) return [];
			var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
			foreach (var session in MappedSessions)
				session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
			return MappedSessions;
			//return sessions.Select(X => new SessionViewModel()
			//{
			//	Id = X.Id,
			//	Description = X.Description,
			//	CategoryName = X.Category.CategoryName,
			//	Capacity = X.Capacity,
			//	StartDate = X.StartDate,
			//	EndDate = X.EndDate,
			//	TrainerName = X.Trainer.Name,
			//	AvailableSlots = X.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(X.Id)

			//});

		}
		public bool CreateSession(CreateSessionViewModel createSession)
		{
			
			try
			{
				if (!IsTrainerExist(createSession.TrainerId)) return false;
				if (!IsCategoryExist(createSession.CategoryId)) return false;
				if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;

				if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;
				var sessionEntity = mapper.Map<Session>(createSession);
				unitOfWork.GetRepository<Session>().Add(sessionEntity);
				return unitOfWork.SaveChanges() > 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"Creation Failed : {ex}");
				return false;
			}
		}

		public SessionViewModel? GetSessionById(int sessionId)
		{
			var session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryById(sessionId);
			if (session is null) return null;
			var MappedSession = mapper.Map<Session, SessionViewModel>(session);
			return MappedSession;
		}

		public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
		{
			var session = sessionRepository.GetById(sessionId);
			if(session is null) return null;
			if(!IsSessionAvailableForUpdating(session)) return null;
			var mappedSession = mapper.Map<UpdateSessionViewModel>(session);
			return mappedSession;
		}

		public bool UpdateSession(int sessionId, UpdateSessionViewModel UpdatedSession)
		{
			try
			{
				var session = unitOfWork.SessionRepository.GetById(sessionId);
				if (session is null) return false;
				if(!IsSessionAvailableForUpdating(session)) return false;
				if(!IsTrainerExist(UpdatedSession.TrainerId)) return false;
				//if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false ; 
				mapper.Map(UpdatedSession,session);
				unitOfWork.SessionRepository.Update(session);
				return unitOfWork.SaveChanges() > 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine($"Update Session Failed : {ex}");
				return false;
			}
		}
		public bool RemoveSession(int sessionId)
		{
			try
			{
				var session = unitOfWork.GetRepository<Session>().GetById(sessionId);
				if (session is null) return false;
				if(!IsSessionAvailableForRemoving(session)) return false;
				unitOfWork.GetRepository<Session>().Delete(session);
				return unitOfWork.SaveChanges() > 0;

			}
			catch (Exception ex)
			{

				Console.WriteLine($"Remove Session Failed : {ex}");
				return false;
			}
		}


		#region Helper Method
		private bool IsSessionAvailableForUpdating(Session session)
		{
			if (session is null) return false;
			if (session.EndDate < DateTime.Now) return false;
			if (session.StartDate <= DateTime.Now) return false;

			var hasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
			if (hasActiveBooking) return false;
			return true;

		}
		private bool IsSessionAvailableForRemoving(Session session)
		{
			if (session is null) return false;
			
			if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
			if (session.StartDate > DateTime.Now) return false;



			var hasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
			if (hasActiveBooking) return false;
			return true;

		}

		private bool IsTrainerExist(int TrainerId)
		{
			return unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
		}
		private bool IsCategoryExist(int CategoryId)
		{
			return unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
		}
		private bool IsDateTimeValid(DateTime startDate , DateTime endDate)
		{
			return startDate < endDate;
		}

		


		#endregion
	}
}
	
