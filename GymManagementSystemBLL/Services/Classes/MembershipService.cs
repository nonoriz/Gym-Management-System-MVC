using AutoMapper;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.MembershipViewModels;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Classes;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MembershipService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public bool CreateMembership(CreateMembershipViewModel CreatedMemberShip)
        {
            try
            {
                if (!IsMemberExists(CreatedMemberShip.MemberId) || !IsPlanExists(CreatedMemberShip.PlanId)
                    || HasActiveMemberShip(CreatedMemberShip.MemberId)) return false;
                var MemberShipToCreate = mapper.Map<MemberShip>(CreatedMemberShip);
                var Plan = unitOfWork.GetRepository<Plan>().GetById(CreatedMemberShip.PlanId);
                MemberShipToCreate.EndDate = DateTime.Now.AddDays(Plan!.DurationDays);
                unitOfWork.MembershipRepository.Add(MemberShipToCreate);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMemberShip(int MemberId)
        {
            var Repo = unitOfWork.MembershipRepository;
            var ActiveMemberships = Repo.GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();
            if (ActiveMemberships is null) return false;
            Repo.Delete(ActiveMemberships);
            return unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberShips()
        {
            var MemberShips = unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlan(X => X.Status == "Active");
            if (!MemberShips.Any()) return [];
            return mapper.Map<IEnumerable<MembershipViewModel>>(MemberShips);
        }

        public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown()
        {
            var Members = unitOfWork.GetRepository<Member>().GetAll();
            return mapper.Map<IEnumerable<MemberSelectListViewModel>>(Members);
        }

        public IEnumerable<PlanSelectListViewModel> GetPlansForDropDown()
        {
            var Plans = unitOfWork.GetRepository<Plan>().GetAll(X => X.IsActive == true);
            return mapper.Map<IEnumerable<PlanSelectListViewModel>>(Plans);
        }


        #region Helper Methods 

        private bool IsMemberExists(int MemberId)
        {
            return unitOfWork.GetRepository<Member>().Exists(X => X.Id == MemberId);
        }
        private bool IsPlanExists(int PlanId)
        {
            return unitOfWork.GetRepository<Plan>().Exists(X => X.Id == PlanId);
        }
        private bool HasActiveMemberShip(int memberId)
        {
            return unitOfWork.MembershipRepository.Exists(X => X.MemberId == memberId && X.Status == "Active");
        }


        #endregion
    }
}
