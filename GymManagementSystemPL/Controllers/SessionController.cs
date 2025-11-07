using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystemPL.Controllers
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
            var sessions = sessionService.GetAllSession();
            return View(sessions);
        }
        #endregion

        #region Get Session Details 

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionById(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        #endregion

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDownForCategories();
            LoadDropDownForTrainers();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownForCategories();
                LoadDropDownForTrainers();
                return View(createSession);
            }
            var result = sessionService.CreateSession(createSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Can Not Be Created";
                LoadDropDownForCategories();
                LoadDropDownForTrainers();
                return View(createSession);

            }
        }



        #endregion

        #region Edit Session

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id is not valid";
                return RedirectToAction(nameof(Index));
            }
            var session=sessionService.GetSessionToUpdate(id);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not be Updated";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownForTrainers();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id , UpdateSessionViewModel updateSession)
        {
            if(!ModelState.IsValid)
            {
                LoadDropDownForTrainers();
                return View(updateSession);
            }
            var result = sessionService.UpdateSession( updateSession,id);
            if (result)
            {
                TempData["SeuccessMessage"] = "Session Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To update";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Session


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id is not valid";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMrssage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;

            return View(session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var result = sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Delete";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Helper Methods

        private void LoadDropDownForCategories()
        {
            var Categories = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

          
        }
        private void LoadDropDownForTrainers()
        {
           

            var Trainers = sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        #endregion
    }
}
