using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IAnalyticService _analyticService;

		public HomeController(IAnalyticService analyticService)
		{
			_analyticService = analyticService;
		}
		public ViewResult Index()
		{
			var Data = _analyticService.GetAnalyticsData();
			return View(Data);
		}
		
	}
}
