using GymManagementSystemBLL.Services.Classes;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        #region Get All Trainers
        public IActionResult Index()
        {
            var Trainers = trainerService.GettAllTrainers();
            return View(Trainers);
        }
        #endregion

        #region Create Trainer

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Feilds");
                return View(nameof(Create), createTrainer);
            }
            var Result=trainerService.CreateTrainer(createTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed to be Creates";
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Trainer Details

        public ActionResult TrainerDetails(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));
            }
            var Trainer=trainerService.GetTrainerDetails(id);
            if(Trainer is null)
            {
                TempData["ErrorMrssage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        #endregion

        #region Edit Trainer 

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id Is InValid";
                return RedirectToAction(nameof(Index));
            }
            var trainer=trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMrssage"] = "trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit(int id,TrainerToUpdateViewModel updateTrainer)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTrainer);
            }
            var Result = trainerService.UpdateTrainerDetails(id, updateTrainer);
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

        #region Delete Trainer

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id of member can not be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }
            var Member = trainerService.GetTrainerDetails(id);
            if (Member is null)
            {
                TempData["ErrorMrssage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;

            return View(Member);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = trainerService.DeleteTrainer(id);
            if (Result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Can Not Be Deleted";
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
