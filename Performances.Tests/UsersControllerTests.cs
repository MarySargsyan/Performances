using Microsoft.AspNetCore.Mvc;
using Moq;
using Perfomans.Controllers;
using Perfomans.Models;
using Perfomans.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Performances.Tests
{
    public class UsersControllerTests
    {
        private List<User> GetTestUsers()
        {
            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";
            string adminName = "Mary";
            string adminSourName = "Sargsyan";

            var users = new List<User>
            {
                new User {Id = 1,Name = adminName,SourName = adminSourName,Email = adminEmail,Password = adminPassword,RoleId = 1,DepartmentId = 1,StateId = 1,SupervisorId = 1 },
                new User {Id = 2,Name = adminName,SourName = adminSourName,Email = adminEmail,Password = adminPassword,RoleId = 1,DepartmentId = 1,StateId = 1,SupervisorId = 1 }
            };
            return users;
        }
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.AllUsers()).Returns(GetTestUsers());
            var controller = new UsersController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.Equal(GetTestUsers().Count, model.Count());
        }
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsersNotNull()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            mock.Setup(service => service.AllUsers()).Returns(GetTestUsers());
            var controller = new UsersController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.NotNull(model);
        }
        [Fact]
        public void AddUserReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            var controller = new UsersController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            User newUser = new User();

            // Act
            var result = controller.Create(newUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult?.Model);
        }
        [Fact]
        public void AddUserReturnsARedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IUserService>();
            var controller = new UsersController(mock.Object);
            var newUser = new User()
            {
                Name = "Ben"
            };

            // Act
            var result = controller.Create(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Insert(newUser));
        }

        [Fact]
        public void UsersEditReturnsEqualtestid()
        {
            int testUserId = 1;
            var mock = new Mock<IUserService>();
            mock.Setup(repo => repo.GetById(testUserId))
                .Returns(GetTestUsers().FirstOrDefault(p => p.Id == testUserId));
            var controller = new UsersController(mock.Object);
            // Act
            var result = controller.Edit(testUserId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.ViewData.Model);
            Assert.Equal(testUserId, model.Id);
            Assert.Equal("Mary", model.Name);
        }
    }
}
