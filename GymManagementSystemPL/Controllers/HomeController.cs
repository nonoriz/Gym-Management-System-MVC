using GymManagementSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            this.analyticsService = analyticsService;
        }
        public ViewResult Index()
        {
            var Data = analyticsService.GetAnalyticsData();
            return View(Data);
        }

        //public RedirectResult Redirect()
        //{
        //    return Redirect("https://foregoing-sunshine-22c.notion.site/Dot-Net-Back-End-Developer-RoadMap-108abbeedc9580888d75e84ba465e9fd");

        //}

        //public ContentResult Content()
        //{
        //    return Content("Hell From Gym Management System");
        //}
    }
}
