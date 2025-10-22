using GymManagementSystemBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Interfaces
{
    public interface IPanService
    {
        IEnumerable<PlanViewModel> GetPlans();

        PlanViewModel? GetPlanById(int PlanId);

        UpdatePlanViewModel? GetUpdatePlanViewModel(int PlanId);

        bool updatedPlan (int PlanId , UpdatePlanViewModel updatedPlan);

        bool ToggleStatus (int PlanId);
    }
}
