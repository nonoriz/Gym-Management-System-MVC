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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
           dbContext.Set<TEntity>().Add(entity);
           
        }

        public void Delete(TEntity  entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
           
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition == null) 
                return dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
                return dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();

        }
        public bool Exists(Func<TEntity, bool> predicate)
        {
            return dbContext.Set<TEntity>().Any(predicate);
        }


        public TEntity? GetById(int id)
        => dbContext.Set<TEntity>().Find(id);

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
           
        }
    }
}
