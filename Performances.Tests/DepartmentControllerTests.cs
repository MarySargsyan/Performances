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
    public class DepartmentControllerTests
    {
        
        private List<Departments> GetTestDepartments()
        {
            var departments = new List<Departments>
            {
                new Departments {Id = 1,Name = "Developing"},
                new Departments {Id = 2,Name ="Marketing"}
            };
            return departments;
        }
        [Fact]
        public void IndexReturnsAViewResultWithAListOfDepartments()
        {
            // Arrange
            var mock = new Mock<IDepartmentsService>();
            var mock1 = new Mock<IGroupService>();
            mock.Setup(service => service.AllDepartments()).Returns(GetTestDepartments());
            var controller = new DepartmentsController(mock.Object, mock1.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Departments>>(viewResult.Model);
            Assert.Equal(GetTestDepartments().Count, model.Count());
        }
        [Fact]
        public void AddDepartmentsReturnsViewResultWithDepartmentModel()
        {
            // Arrange
            var mock = new Mock<IDepartmentsService>();
            var mock1 = new Mock<IGroupService>();
            var controller = new DepartmentsController(mock.Object, mock1.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Departments departments = new Departments();

            // Act
            var result = controller.Create(departments);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(departments, viewResult?.Model);
        }
        [Fact]
        public void AddParameterReturnsARedirectAndAddParameter()
        {
            // Arrange
            var mock = new Mock<IDepartmentsService>();
            var mock1 = new Mock<IGroupService>();
            var controller = new DepartmentsController(mock.Object, mock1.Object);
            Departments departments = new Departments()
            {
                Name = "Developing"
            };

            // Act
            var result = controller.Create(departments);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Insert(departments));
        }
        [Fact]
        public void DeleteDepartmentReturnsDeleteDepartment()
        {
            // Arrange
            int testDepartmentId = 1;
            var mock = new Mock<IDepartmentsService>();
            mock.Setup(repo => repo.GetById(testDepartmentId)).Returns(GetTestDepartments().FirstOrDefault(p => p.Id == testDepartmentId));
            var mock1 = new Mock<IGroupService>();
            var controller = new DepartmentsController(mock.Object, mock1.Object);

            // Act
            var result = controller.Delete(testDepartmentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Departments>(viewResult.ViewData.Model);
            Assert.Equal(testDepartmentId, model.Id);
        }

        [Fact]
        public void DepartmentPageReturn()
        {
            int testDepartmentId = 1;
            var mock = new Mock<IDepartmentsService>();
            mock.Setup(repo => repo.GetById(testDepartmentId)).Returns(GetTestDepartments().FirstOrDefault(p => p.Id == testDepartmentId));
            var mock1 = new Mock<IGroupService>();
            var controller = new DepartmentsController(mock.Object, mock1.Object);

            // Act
            var result = controller.Edit(testDepartmentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Departments>(viewResult.ViewData.Model);
            Assert.Equal(testDepartmentId, model.Id);
        }
        [Fact]
        public void IndexNotNullReturn()
        {
            var mock = new Mock<IDepartmentsService>();
            var mock1 = new Mock<IGroupService>();
            var controller = new DepartmentsController(mock.Object, mock1.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
