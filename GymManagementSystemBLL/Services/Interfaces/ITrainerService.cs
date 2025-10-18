using GymManagementSystemBLL.ViewModels.MemberViewModels;
using GymManagementSystemBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GettAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel CreatedModel);

        TrainerViewModel? GetTrainerDetails(int TrainerId);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);
        bool UpdateTrainerDetails(int Id, TrainerToUpdateViewModel UpdatedTrainer);

        bool DeleteTrainer(int TrainerId);
    }
}
