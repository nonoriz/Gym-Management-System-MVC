using GymManagementSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = . ; Database = GymManagement ; Trusted_Connection = true ; TrustServerCertificate = true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.Property(u => u.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
                eb.Property(u => u.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            });
        }

        #region DbSets
        

        public DbSet<Member> Members { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<MemberShip> Memberships { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Plan> Plans { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        #endregion
    }
}
