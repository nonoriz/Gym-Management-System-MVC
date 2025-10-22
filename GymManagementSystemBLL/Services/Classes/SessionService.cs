using AutoMapper;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                if (!IsTrainerExists(createdSession.TrainerId) || !IsCategoryExists(createdSession.CategoryId) || !IsDateTimeValid(createdSession.StartDate, createdSession.EndDate))
                    return false;
                var session = mapper.Map<Session>(createdSession);
                unitOfWork.SessionRepository.Add(session);
                return unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
            

        }

        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var Sessions =unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();

            if (!Sessions.Any()) return [];

            var MappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session  = unitOfWork.SessionRepository.GetById(sessionId);
            if (session == null) return null;

            var MappedSession=mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;


        }


        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForUpdating(session!)) return null;
            var MappedSession = mapper.Map<Session, UpdateSessionViewModel>(session!);
            return MappedSession;

        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var Session = unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdating(Session!)) return false;
                if (!IsTrainerExists(updatedSession.TrainerId) || !IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate))
                    return false;
                var MappedSession = mapper.Map(updatedSession, Session!);
                Session!.UpdatedAt = DateTime.Now;
                unitOfWork.SessionRepository.Update(MappedSession);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        public bool RemoveSession(int sessionId)
        {
            try
            {
                var session = unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForRemoving(session!)) return false;
                unitOfWork.SessionRepository.Delete(session!);
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
           var trainers= unitOfWork.GetRepository<Trainer>().GetAll();
            if (!trainers.Any()) return [];
            var MappedTrainers = mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
            return MappedTrainers;
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
           var categories= unitOfWork.GetRepository<Category>().GetAll();
            if (!categories.Any()) return [];
            var MappedCategories = mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories);
            return MappedCategories;
        }




        #region Helper Methods

        private bool IsSessionAvailableForUpdating(Session session)
        {
            if(session is null) return false;

            if(session.EndDate<DateTime.Now) return false;

            if(session.StartDate<=DateTime.Now) return false;

            var HasActiveBookings = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;

            return true;
        }


        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session is null) return false;


            if (session.StartDate <= DateTime.Now && session.EndDate>DateTime.Now) return false;

            if (session.StartDate > DateTime.Now) return false;

            var HasActiveBookings = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBookings) return false;

            return true;
        }

        private bool IsTrainerExists(int trainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            return trainer != null;
        }
        private bool IsCategoryExists(int categoryId)
        {
            var category = unitOfWork.GetRepository<Category>().GetById(categoryId);
            return category != null;
        }
        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && DateTime.Now <startDate;
        }

       




        #endregion
    }
}
