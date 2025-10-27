using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Models
{
    public class MemberShip: BaseEntity
    {
        #region Properties
        
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if(EndDate <= DateTime.Now)
                {
                    return "Expired";
                }
                else
                {
                    return "Active";
                }
            }
        }
        
        #endregion


        #region Navigation Properties

        #region Member - Memberships
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        #endregion

        #region Plan - Memberships
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        #endregion

        #endregion

    }
}
