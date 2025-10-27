using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        #region Get All Members
        public ActionResult Index()
        {
            var members = memberService.GettAll();
            return View(members);
        }
        #endregion

        #region Get Member Data

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id of member can not be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }
            var Member=memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMrssage"] = "Member Nor Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id of member can not be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord=memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord is null)
            {
                TempData["ErrorMrssage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }


        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Feilds");
                return View(nameof(Create),createdMember);
            }
            bool Result=memberService.CreateMember(createdMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member created successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to creating , check phone and email ";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id of member can not be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }
            var Member = memberService.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMrssage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id,MemberToUpdateViewModel memberToEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(memberToEdit);
            }
            var Result=memberService.UpdateMemberDetails(id, memberToEdit);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update";
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Delete Member 

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id of member can not be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }
            var Member = memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMrssage"] = "Member Nor Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.memberId = id;

            return View(Member);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = memberService.DeleteMember(id);
            if (Result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Can Not Be Deleted";
            return RedirectToAction(nameof(Index));
        }


        #endregion
    }
}
