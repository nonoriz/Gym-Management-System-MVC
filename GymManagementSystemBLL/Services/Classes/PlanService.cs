using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.PlanViewModels;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace GymManagementSystemBLL.Services.Classes
{
    public class PlanService : IPanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public PlanViewModel? GetPlanById(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if(plan == null) return null;
      
            var viewModel=mapper.Map<Plan, PlanViewModel>(plan);
            return viewModel;
        }

        public IEnumerable<PlanViewModel> GetPlans()
        {
            var Plans = unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any() ) return [];
        
            var viewModels=mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(Plans);
            return viewModels;
        }

        public UpdatePlanViewModel? GetUpdatePlanViewModel(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberships(PlanId)) return null;
           
            var viewModel=mapper.Map<Plan, UpdatePlanViewModel>(plan);
            return viewModel;
        }

        public bool ToggleStatus(int PlanId)
        {
            var Repo = unitOfWork.GetRepository<Plan>();
            var plan = Repo.GetById(PlanId);
            if (plan == null || HasActiveMemberships(PlanId)) return false;
            plan.IsActive=plan.IsActive==true?false:true;
            plan.UpdatedAt=DateTime.Now;
            try
            {
                Repo.Update(plan);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool updatedPlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || HasActiveMemberships(PlanId)) return false;
            try
            {
                var MappedPlan = mapper.Map(updatedPlan, plan);
                unitOfWork.GetRepository<Plan>().Update(MappedPlan);
                return unitOfWork.SaveChanges() > 0;
            }
            catch {  return false; }

        }


        #region Helper

        private bool HasActiveMemberships(int PlanId)
        {
            var ActiveMemberships= unitOfWork.GetRepository<MemberShip>()
                .GetAll(x=>x.PlanId==PlanId && x.Status=="Active");
            return ActiveMemberships.Any();
        }

        #endregion
    }
}
