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
        public DbSet<DepartmentParameters> DepartmentParameters { get; set; }
        public DbSet<DepartmentHead> Heads { get; set; }
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

            modelBuilder.Entity<DepartmentHead>().HasKey(d => new { d.DepartmentId, d.HeadId });
            modelBuilder.Entity<DepartmentHead>().HasOne(d => d.Departments).WithMany(d => d.Head).HasForeignKey(d => d.DepartmentId);
            modelBuilder.Entity<DepartmentHead>().HasOne(d => d.Head).WithMany(d => d.DeportamentHead).HasForeignKey(d => d.HeadId);

            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";
            string adminName = "Mary";
            string adminSourName = "Sargsyan";

            string devdep = "Developing";

            string DefState = "Default";
            string FireState = "Fired";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };

            Departments departments = new Departments { Id = 1, Name = devdep };

            State state = new State { Id = 1, Name = DefState };
            State state1 = new State { Id = 1, Name = FireState };


            User adminUser = new User { Id = 1, Name = adminName, SourName = adminSourName, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id, DepartmentId= departments.Id, StateId = state.Id, SupervisorId = 1 };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
