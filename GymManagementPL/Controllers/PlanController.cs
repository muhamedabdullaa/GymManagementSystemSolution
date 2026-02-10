using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	[Authorize]
	public class PlanController : Controller
	{
		private readonly IPlanService planService;

		public PlanController(IPlanService planService)
		{
			this.planService = planService;
		}
		#region GetAll Plans
		public ActionResult Index()
		{
			var plans = planService.GetAllPlans();
			return View(plans);
		}
		#endregion
		#region Plan Details
		public ActionResult Details(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Plan Id Not Available";
				return RedirectToAction(nameof(Index));
			}
			var plan = planService.GetPLanById(id);
			if (plan == null)
			{
				TempData["ErrorMessage"] = "Plan Not Found";
				return RedirectToAction(nameof(Index));
			}
			return View(plan);


		}
		#endregion
		#region Edit Plan
		public ActionResult Edit(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Plan Id Not Available";
				return RedirectToAction(nameof(Index));
			}
			var plan = planService.GetPlanToUpdate(id);
			if (plan == null)
			{
				TempData["ErrorMessage"] = "Plan Can Not Be Updated";
				return RedirectToAction(nameof(Index));
			}

			return View(plan);
		}
		[HttpPost]
		public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("WrongData" ,"Check Data Validation");
				return View(UpdatedPlan);
			}
			var Result = planService.UpdatePlan(id, UpdatedPlan);
			if (Result)
			{
				TempData["SuccessMessage"] = "Plan Updated";

			}
			else
			{
				TempData["ErrorMessage"] = "Plan Failed To Update";
			}
			return RedirectToAction(nameof(Index));
		}
		#endregion
		#region Delete Plan
		//Active
		[HttpPost]
		public ActionResult Activate(int id)
		{
			var Result = planService.ToggleStatus(id);
			if (Result)
			{
				TempData["SuccessMessage"] = "Plan Status Changed";
			}
			else
			{
				TempData["ErrorMessage"] = "Plan Status Doesn't Changed";
			}
			return RedirectToAction(nameof(Index));
		}
		#endregion
	}
}
