using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	//[Authorize(Roles = "SuperAdmin")]
	public class TrainerController : Controller
	{
		private readonly ITrainerService trainerService;

		public TrainerController(ITrainerService trainerService)
		{
			this.trainerService = trainerService;
		}
		#region Get All Trainers
		public ActionResult Index()
		{
			var trainers = trainerService.GetAllTrainers();
			return View(trainers);
		}
		#endregion
		#region Get Trainer 
		public ActionResult Details(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Invalid Trainer Id";
				return RedirectToAction(nameof(Index));
			}
			var trainer = trainerService.GetTrainerById(id);
			if (trainer is null)
			{
				TempData["ErrorMessage"] = "Can Not Find Trainer";
				return RedirectToAction(nameof(Index));
			}
			return View(trainer);
		} 
		#endregion
		#region Create Trainer
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("DataMissed", "Check Missing Fields");
				return View(nameof(Create), createTrainer);
			}
			var Result = trainerService.CreateTrainer(createTrainer);
			if (Result)
			{
				TempData["SuccessMessage"] = "Trainer Created Successfully";
			}
			else
			{
				TempData["ErrorMessage"] = "Trainer Can Not Be Created";

			}
			return RedirectToAction(nameof(Index));
		}
		#endregion
		#region Edit Trainer
	
		public ActionResult Edit(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
			var trainer = trainerService.TrainerToUpdate(id);
			if (trainer is null)
			{
				TempData["ErrorMessage"] = "Trainer  Is Not Found";
				return RedirectToAction(nameof(Index));
			}
			return View(trainer);
		}
		[HttpPost]
		public ActionResult Edit([FromRoute] int id, UpdateTrainerViewModel TrainerToEdit)
		{
			if (!ModelState.IsValid)
				return View(TrainerToEdit);
			var Result = trainerService.UpdateTrainer(id, TrainerToEdit);
			if (Result)
			{
				TempData["SuccessMessage"] = "Trainer Updated Successfully";
			}
			else
			{
				TempData["ErrorMessage"] = "Trainer Failed To Update";
			}
			return RedirectToAction(nameof(Index));

		}
		#endregion
		
	}
}
