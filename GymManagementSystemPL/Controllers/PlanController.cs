using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPanService planSercive;

        public PlanController(IPanService planSercive)
        {
            this.planSercive = planSercive;
        }

        #region Get All Plans
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Get Plan Details
        
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan= planSercive.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        #endregion

        #region Edit plan

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = planSercive.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatedPlan)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(updatedPlan);
            }
            var isUpdated = planSercive.updatedPlan(id, updatedPlan);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Toggle Plan Status

        public ActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var isToggled = planSercive.ToggleStatus(id);
            if (!isToggled)
            {
                TempData["ErrorMessage"] = "Plan Status Can Not Be Changed";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
