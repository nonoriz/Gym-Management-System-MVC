using GymManagementSystemDAL.contexts;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories = new();
        private readonly GymDbContext dbContext;

        public UnitOfWork(GymDbContext dbContext,ISessionRepository sessionRepository,IMembershipRepository membershipRepository,IBookingRepository bookingRepository) {
            this.dbContext = dbContext;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
            BookingRepository = bookingRepository;
        }

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
           var TEntityType =typeof(TEntity);
            if(repositories.TryGetValue(TEntityType, out var repository))
                return (IGenericRepository<TEntity>)repository;

            var NewRepo = new GenericRepository<TEntity>(dbContext);
            repositories[TEntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
           return dbContext.SaveChanges();
        }
    }
}
