using GymManagementSystemBLL.Services.Classes;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IMembershipService membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        #region Get All Memberships
        public ActionResult Index()
        {
            var memberships = membershipService.GetAllMemberShips();
            return View(memberships);
        } 
        #endregion

        #region Create Membership
        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateMembershipViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = membershipService.CreateMembership(model);

                if (result)
                {
                    TempData["SuccessMessage"] = "Membership created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create membership. member have an active membership.";
                }
            }
            LoadDropdowns();
            return View(model);
        }
        #endregion

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var result = membershipService.DeleteMemberShip(id);

            if (result)
            {
                TempData["Success"] = "Membership cancelled successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to cancel membership.";
            }

            return RedirectToAction(nameof(Index));
        }


        #region Helper Methods
        private void LoadDropdowns()
        {
            var members = membershipService.GetMembersForDropDown();
            var plans = membershipService.GetPlansForDropDown();

            ViewBag.members = new SelectList(members, "Id", "Name");
            ViewBag.plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
