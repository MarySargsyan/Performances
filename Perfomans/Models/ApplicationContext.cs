using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<DepartmentHead> DepartmentHead { get; set; }
        public DbSet<DepartmentParameters> DepartmentParameters { get; set; }
        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<Evaluations> Evaluations { get; set; }
        public DbSet<State> States { get; set; } 

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentParameters>().HasKey(pi => new { pi.DepartmentId, pi.ParameterId });
            modelBuilder.Entity<DepartmentParameters>().
                HasOne(pi => pi.Department).WithMany(pi => pi.Parameters).HasForeignKey(p => p.DepartmentId);
            modelBuilder.Entity<DepartmentParameters>().
                HasOne(pi => pi.parameter).WithMany(pi => pi.Departments).HasForeignKey(p => p.ParameterId);

            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";
            string adminName = "Mary";
            string adminSourName = "Sargsyan";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Name = adminName, SourName = adminSourName, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
