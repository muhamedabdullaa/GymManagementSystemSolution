using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
	public interface ITrainerService
	{
		IEnumerable<TrainerViewModel> GetAllTrainers();
		bool CreateTrainer(CreateTrainerViewModel createTrainer);
		TrainerViewModel GetTrainerById(int id);
		UpdateTrainerViewModel TrainerToUpdate(int id);
		bool UpdateTrainer(int Id, UpdateTrainerViewModel? updateTrainer);
		bool DeleteTrainer(int Id);
	}
}
