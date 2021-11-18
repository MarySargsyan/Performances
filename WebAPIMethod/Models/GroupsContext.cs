using Microsoft.EntityFrameworkCore;
using System;
using WebAPIMethod.Models;

namespace WebAPIMethod.Models
{
    public class GroupsContext : DbContext
    {
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<ParametersGroup> ParametersGroups { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Evaluations> Evaluations { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserParamEval> UserParamEval { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParametersGroup>().HasKey(pi => new { pi.GroupId, pi.ParameterId });
            modelBuilder.Entity<ParametersGroup>().
                HasOne(pi => pi.Parameters).WithMany(pi => pi.ParametersGroups).HasForeignKey(p => p.ParameterId);
            modelBuilder.Entity<ParametersGroup>().
                HasOne(pi => pi.Groups).WithMany(pi => pi.ParametersGroups).HasForeignKey(p => p.GroupId);



            string devdep = "Developing";
            string marketing = "Marketing";

            string GroupName = "Future Manager";
            string GroupName1 = "The Best Employees";

            string param1 = "Responsibility level";
            string param2 = "Level of independence";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";
            string adminName = "Mary";
            string adminSourName = "Sargsyan";

            string HeadEmail = "Head@mail.ru";
            string HeadPassword = "123456";
            string HeadName = "Kseniya";
            string HeadSourName = "Svistunova";

            string TeamLeadEmail = "TeamLead@mail.ru";
            string TeamLeadPassword = "123456";
            string TeamLeadName = "Polina";
            string TeamLeadSourName = "Shevchuk";



            Departments department = new Departments { Id = 1, Name = devdep };
            Departments department2 = new Departments { Id = 2, Name = marketing };

            Groups group1 = new Groups { id = 1, Name = GroupName, DepartmentId = department.Id };
            Groups group2 = new Groups { id = 2, Name = GroupName1, DepartmentId = department.Id };

            Parameters parameter = new Parameters { Id = 1, Name = param1 };
            Parameters parameter2 = new Parameters { Id = 2, Name = param2 };

            ParametersGroup parametersGroup = new ParametersGroup { ParameterId = 1, GroupId = 1 };
            ParametersGroup parametersGroup2 = new ParametersGroup { ParameterId = 2, GroupId = 1 };
            ParametersGroup parametersGroup3 = new ParametersGroup { ParameterId = 1, GroupId = 2 };
            ParametersGroup parametersGroup4 = new ParametersGroup { ParameterId = 2, GroupId = 2 };

            User adminUser = new User
            {
                Id = 1,
                Name = adminName,
                SourName = adminSourName,
                Email = adminEmail,
                Password = adminPassword,
                DepartmentId = department.Id,
                SupervisorId = 1
            };
            User HeadUser = new User
            {
                Id = 2,
                Name = HeadName,
                SourName = HeadSourName,
                Email = HeadEmail,
                Password = HeadPassword,
                DepartmentId = department.Id,
                SupervisorId = 1
            };
            User TeamLeadUser = new User
            {
                Id = 3,
                Name = TeamLeadName,
                SourName = TeamLeadSourName,
                Email = TeamLeadEmail,
                Password = TeamLeadPassword,
                DepartmentId = department.Id,
                SupervisorId = 2
            };

            DateTime myDate = DateTime.Parse("25/10/2021");

            Evaluations evaluation = new Evaluations { Id = 1, Date = myDate, AssessorId = 1, UserId= 2 };
            Evaluations evaluation3 = new Evaluations { Id = 3, Date = myDate, AssessorId = 1, UserId= 3};

            modelBuilder.Entity<Departments>().HasData(new Departments[] { department, department2 });
            modelBuilder.Entity<Parameters>().HasData(new Parameters[] { parameter, parameter2 });
            modelBuilder.Entity<ParametersGroup>().HasData(new ParametersGroup[] { parametersGroup, parametersGroup2, parametersGroup3, parametersGroup4 });
            modelBuilder.Entity<Groups>().HasData(new Groups[] { group1, group2});
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, HeadUser, TeamLeadUser });
            modelBuilder.Entity<Evaluations>().HasData(new Evaluations[] { evaluation, evaluation3 });

            base.OnModelCreating(modelBuilder);
        }
          public GroupsContext(DbContextOptions<GroupsContext> options)
              : base(options)
          {
            Database.EnsureCreated();
          }
    }
}
