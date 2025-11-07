using GymManagementSystemBLL.Services.Classes;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        public ActionResult Index()
        {
            var bookings = bookingService.GetAllSessions();
            return View(bookings);
        }
        public ActionResult GetMembersForUpcomingSession(int id)
        {
            var Members = bookingService.GetMembersForUpcomingBySessionId(id);
            return View(Members);
        }


        #region Create 
        public ActionResult Create(int id)
        {
            var members = bookingService.GetMembersForDropDown(id);
            ViewBag.members = new SelectList(members, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateBookingViewModel createdBooking)
        {

            var result = bookingService.CreateNewBooking(createdBooking);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking Created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Booking.";
            }

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = createdBooking.SessionId });


        }

        #endregion


        #region Cancel


        [HttpPost]
        public ActionResult Cancel(int MemberId, int SessionId)
        {
            var result = bookingService.CancelBooking(MemberId, SessionId);
            if (result)
            {
                TempData["SuccessMessage"] = "Booking cancelled successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel Booking.";
            }

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = SessionId });
        } 
        #endregion
        public ActionResult GetMembersForOngoingSessions(int id)
        {
            var Members = bookingService.GetMembersForOngoingBySessionId(id);
            return View(Members);
        }
        [HttpPost]
        public ActionResult Attended(int MemberId, int SessionId)
        {
            var result = bookingService.MemberAttended(MemberId, SessionId);
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = SessionId });

        }
    }
}
