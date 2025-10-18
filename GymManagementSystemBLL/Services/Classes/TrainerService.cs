using AutoMapper;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.MemberViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.TrainerViewModels;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;


namespace GymManagementSystemBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService( IUnitOfWork unitOfWork , IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel CreatedModel)
        {
            try
            {
                if (IsEmailExists(CreatedModel.Email) || IsPhoneExists(CreatedModel.Phone)) return false;

               
                var trainer = mapper.Map<CreateTrainerViewModel, Trainer>(CreatedModel);
                unitOfWork.GetRepository<Trainer>().Add(trainer);
                return unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainer=unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;

          
            var ViewModel = mapper.Map<Trainer, TrainerViewModel>(trainer);
            return ViewModel;
            
        }

        public IEnumerable<TrainerViewModel> GettAllTrainers()
        {
            var Trainers = unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers == null || !Trainers.Any()) return [];

          
            var ViewTrainer = mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(Trainers);
            return ViewTrainer;

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            var ViewModel = mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return ViewModel;

        }

        public bool UpdateTrainerDetails(int Id, TrainerToUpdateViewModel UpdatedTrainer)
        {
            try
            {
                var EmailExists = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == UpdatedTrainer.Email && x.Id != Id);

                var PhoneExists = unitOfWork.GetRepository<Trainer>().GetAll(x => x.PhoneNumber == UpdatedTrainer.Phone && x.Id != Id);

                if (EmailExists.Any() || PhoneExists.Any()) return false;

                var TrainerToUpdate = unitOfWork.GetRepository<Trainer>().GetById(Id);
                if (TrainerToUpdate == null) return false;

                var MappedTrainer = mapper.Map(UpdatedTrainer, TrainerToUpdate);
                unitOfWork.GetRepository<Trainer>().Update(MappedTrainer);
                return unitOfWork.SaveChanges() > 0;
            } catch { return false; }

        }



        public bool DeleteTrainer(int TrainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return false;
            var HasActiveSessions = unitOfWork.GetRepository<Session>().GetAll(x => x.TrainerId == TrainerId && x.StartDate > DateTime.Now).Any();
            if (HasActiveSessions) return false;

            try
            {
                unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }



        #region HelperMethod

        private bool IsEmailExists(string Email)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == Email).Any();
        }

        private bool IsPhoneExists(string Phone)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(x => x.PhoneNumber == Phone).Any();
        }

       


        #endregion


    }
}
