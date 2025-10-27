using GymManagementSystemDAL.contexts;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class SessionRepository :GenericRepository<Session> , ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext) :base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null)
        {
            if (condition is null)
                return dbContext.Sessions.Include(X => X.Trainer)
                    .Include(X => X.Category)
                    .ToList();
            else
                return dbContext.Sessions.Include(X => X.Trainer)
                    .Include(X => X.Category)
                    .Where(condition).ToList();
        }

        public int GetCountOfBookedSlots(int SessionId)
        {
            return dbContext.MemberSessions.Count(x=>x.SessionId == SessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return dbContext.Sessions.Include(x => x.Trainer)
                     .Include(x => x.Category)
                     .FirstOrDefault(x => x.Id == sessionId);
        }
    }
}
