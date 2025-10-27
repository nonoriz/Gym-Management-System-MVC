using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Models
{
    public class Member : GymUser
    {
        #region properties
        public string Photo { get; set; }=null!;
        #endregion


        #region Navigation properties

        #region Member - healthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - Memberships

        public ICollection<MemberShip> MemberPlans { get; set; } = null!;


        #endregion

        #region Member - MemberSessions
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;

        #endregion




        #endregion
    }
}
