using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	[Authorize]
	public class SessionController : Controller
	{
		private readonly ISessionService sessionService;

		public SessionController(ISessionService sessionService)
		{
			this.sessionService = sessionService;
		}
		#region Get All Sessions
		public ActionResult Index()
		{
			var sessions = sessionService.GatAllSessions();
			return View(sessions);
		}
		#endregion
		#region Get Session Details
		public ActionResult Details(int id)
		{
			if(id <= 0)
			{
				TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
			var Result = sessionService.GetSessionById(id);
			if(Result is null)
			{
				TempData["ErrorMessage"] = "Session Not Found";
				return RedirectToAction(nameof(Index));
			}
			return View(Result);
		}
		#endregion
		#region Create Session
		public ActionResult Create()
		{
			return View();
		}
		#endregion
	}
}
