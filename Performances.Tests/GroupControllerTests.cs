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
    public class GroupControllerTests
    {
        private List<Groups> GetTestGroups()
        {
            var groups = new List<Groups>
            {
                new Groups {id = 1,Name = "Group1"},
                new Groups {id = 2,Name ="Group2"}
            };
            return groups;
        }
        private List<ParametersGroup> GetTestGroupsParam()
        {
            var groups = new List<ParametersGroup>
            {
                new ParametersGroup { GroupId=1, ParameterId=1, Mark= 5},
                new ParametersGroup { GroupId=1, ParameterId=2, Mark= 5},
            };
            return groups;
        }
        [Fact]
        public void AddGroupReturnsViewResultWithGroupModel()
        {
            var mock = new Mock<IGroupService>();
            var controller = new GroupsController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Groups group = new Groups();

            // Act
            var result = controller.Create(group,1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(group, viewResult?.Model);
        }

        [Fact]
        public void AddGroupReturnsARedirectAndAddGroup()
        {
            // Arrange
            var mock = new Mock<IGroupService>();
            var controller = new GroupsController(mock.Object);
            Groups group = new Groups()
            {
                Name = "Group1"
            };

            // Act
            var result = controller.Create(group,1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("DepartmentPage", redirectToActionResult.ActionName);
        }
        [Fact]
        public void AddGroupReturnsARedirectAndAddGroupVerify()
        {
            // Arrange
            var mock = new Mock<IGroupService>();
            var controller = new GroupsController(mock.Object);
            Groups group = new Groups()
            {
                Name = "Group1"
            };

            // Act
            var result = controller.Create(group,1);

            // Assert
            mock.Verify(r => r.Insert(group));
        }
        [Fact]
        public void DeleteGoupReturnsDeleteGroup()
        {
            // Arrange
            int testGroupId = 1;
            var mock = new Mock<IGroupService>();
            mock.Setup(repo => repo.GetById(testGroupId)).
                Returns(GetTestGroups().FirstOrDefault(p => p.id == testGroupId));
            var controller = new GroupsController(mock.Object);

            // Act
            var result = controller.Delete(testGroupId, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Groups>(viewResult.ViewData.Model);
        }
        [Fact]
        public void DeleteGoupReturnsDeleteGroupEqualtestGeoupId()
        {
            // Arrange
            int testGroupId = 1;
            var mock = new Mock<IGroupService>();
            mock.Setup(repo => repo.GetById(testGroupId)).
                Returns(GetTestGroups().FirstOrDefault(p => p.id == testGroupId));
            var controller = new GroupsController(mock.Object);

            // Act
            var result = controller.Delete(testGroupId, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Groups>(viewResult.ViewData.Model);
            Assert.Equal(testGroupId, model.id);
        }
        [Fact]
        public void DeleteConfirmDPVerifyResult()
        {
            // Arrange
            int testGroupId = 1;
            var mock = new Mock<IGroupService>();
            mock.Setup(repo => repo.GetById(testGroupId)).
                Returns(GetTestGroups().FirstOrDefault(p => p.id == testGroupId));
            var controller = new GroupsController(mock.Object);

            // Act
            var result = controller.DeleteConfirmed(testGroupId, 1);

            // Assert
            mock.Verify(r => r.Delete(testGroupId));
        }
    }
}
