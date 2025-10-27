using GymManagementSystemBLL.ViewModels.BookingViewModels;
using GymManagementSystemBLL.ViewModels.MembershipViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        IEnumerable<MemberForSessionViewModel> GetMembersForUpcomingBySessionId(int sessionId);
        IEnumerable<MemberForSessionViewModel> GetMembersForOngoingBySessionId(int sessionId);
        IEnumerable<MemberSelectListViewModel> GetMembersForDropDown(int sessionId);
        bool CancelBooking(int MemberId, int SessionId);
        bool CreateNewBooking(CreateBookingViewModel createdBooking);
        bool MemberAttended(int MemberId, int SessionId);
    }
}
