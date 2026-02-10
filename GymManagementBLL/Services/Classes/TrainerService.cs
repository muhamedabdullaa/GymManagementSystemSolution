using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	public class TrainerService : ITrainerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper mapper;

		public TrainerService(IUnitOfWork unitOfWork ,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			this.mapper = mapper;
		}
		public bool CreateTrainer(CreateTrainerViewModel createTrainer)
		{
			if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone) || createTrainer is null) return false;
			var trainer = mapper.Map<Trainer>(createTrainer);
			//var trainer = new Trainer()
			//{
			//	Name = createTrainer.Name,
			//	Email = createTrainer.Email,
			//	Phone = createTrainer.Phone,
		
			//	Gender = createTrainer.Gender,
			//	DateOfBirth = createTrainer.DateOfBirth,
			//	CreatedAt = DateTime.Now,
			//	Specialist = createTrainer.Specialists,

			//}; 
			_unitOfWork.GetRepository<Trainer>().Add(trainer);
			return _unitOfWork.SaveChanges() > 0;
					
		}

		public IEnumerable<TrainerViewModel> GetAllTrainers()
		{
			var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
			if (Trainers is null || !Trainers.Any()) return [];
			var MappTrainers = mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
			//return Trainers.Select(X => new TrainerViewModel()
			//{
			//	Name = X.Name,
			//	Email = X.Email,
			//	Phone = X.Phone,
			//	Sppecialization = X.Specialist.ToString()
			//});
			return MappTrainers;
		}

		public TrainerViewModel GetTrainerById(int id)
		{
			var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
			if (trainer is null) return null; 
			//return new TrainerViewModel() 
			//{
			//	Name = trainer.Name,
			//	Email = trainer.Email,
			//	Phone = trainer.Phone,
			//	Sppecialization = trainer.Specialist.ToString()
			//};
			var mappedTrainer = mapper.Map<TrainerViewModel>(trainer);
			return mappedTrainer;
		}

		public UpdateTrainerViewModel TrainerToUpdate(int id)
		{
			var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
			if (trainer is null) return null;
			return new UpdateTrainerViewModel()
			{
				Name = trainer.Name,
				Email = trainer.Email,
				Phone = trainer.Phone,
				Address = trainer.Address,
				Specialist = trainer.Specialist
			};

		}

		public bool UpdateTrainer(int id, UpdateTrainerViewModel? updateTrainer)
		{
			if(updateTrainer is  null || IsEmailExist(updateTrainer.Email) || IsPhoneExist(updateTrainer.Phone)) return false;
			var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
			if (trainer is null) return false;
			(trainer.Name, trainer.Email, trainer.Phone, trainer.Specialist, trainer.Address)
				= (updateTrainer.Name, updateTrainer.Email, updateTrainer.Phone, updateTrainer.Specialist, updateTrainer.Address);
			_unitOfWork.GetRepository<Trainer>().Update(trainer);
			return _unitOfWork.SaveChanges() > 0; 
		}

		public bool DeleteTrainer(int Id)
		{
			var trainer = _unitOfWork.GetRepository<Trainer>().GetById(Id);
			if (trainer is null || HasFutureSession(Id)) return false;
			_unitOfWork.GetRepository<Trainer>().Delete(trainer);
			return _unitOfWork.SaveChanges() > 0;
			
		}

		#region Helper Method
		bool IsEmailExist(string email)
		{
			return _unitOfWork.GetRepository<Trainer>().GetAll(X=> X.Email == email).Any();	
		}
		bool IsPhoneExist(string phone)
		{
			return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == phone).Any();
		}
		bool HasFutureSession(int id)
		{
			return _unitOfWork.GetRepository<Session>().GetAll(X=>X.TrainerId == id && X.StartDate >= DateTime.Now).Any(); 
			
		}

	
		#endregion
	}
}
