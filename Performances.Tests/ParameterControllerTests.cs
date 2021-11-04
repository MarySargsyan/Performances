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
   public class ParameterControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfDepartments()
        {
            // Arrange
            var mock = new Mock<IParametersService>();
            mock.Setup(service => service.AllParameters()).Returns(GetTestParameters());
            var controller = new ParametersController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Parameters>>(viewResult.Model);
            Assert.Equal(GetTestParameters().Count, model.Count());
            Assert.NotNull(model);
        }
        private List<Parameters> GetTestParameters()
        {
            var departments = new List<Parameters>
            {
                new Parameters { Id=1, Name="1", Coefficient= 10, Mark_1_description= "qq", Mark_2_description = "qqq", Mark_3_description="qqqq", Mark_4_description="qqqqq", Mark_5_description="qqqqqq"},
                new Parameters { Id=2, Name="2", Coefficient= 10, Mark_1_description= "qq", Mark_2_description = "qqq", Mark_3_description="qqqq", Mark_4_description="qqqqq", Mark_5_description="qqqqqq"},
                new Parameters { Id=2, Name="3", Coefficient= 10, Mark_1_description= "qq", Mark_2_description = "qqq", Mark_3_description="qqqq", Mark_4_description="qqqqq", Mark_5_description="qqqqqq"},
            };
            return departments;
        }
        [Fact]
        public void AddUserReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IParametersService>();
            var controller = new ParametersController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Parameters newParam = new Parameters();

            // Act
            var result = controller.Create(newParam);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newParam, viewResult?.Model);
        }
        [Fact]
        public void AddParameterReturnsARedirectAndAddParameter()
        {
            // Arrange
            var mock = new Mock<IParametersService>();
            var controller = new ParametersController(mock.Object);
            var newParam = new Parameters()
            {
                Name = "1"
            };

            // Act
            var result = controller.Create(newParam);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Insert(newParam));
        }
        [Fact]
        public void DeleteParameterReturnsDeleteParameter()
        {
            // Arrange
            int testParamId = 1;
            var mock = new Mock<IParametersService>();
            mock.Setup(repo => repo.GetById(testParamId))
                .Returns(GetTestParameters().FirstOrDefault(p => p.Id == testParamId));
            var controller = new ParametersController(mock.Object);


            // Act
            var result = controller.Delete(testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Parameters>(viewResult.ViewData.Model);
            Assert.Equal(testParamId, model.Id);
        }
        [Fact]
        public void GetParameterReturnsViewResultWithParameter()
        {
            // Arrange
            int testParamId = 1;
            var mock = new Mock<IParametersService>();
            mock.Setup(repo => repo.GetById(testParamId))
                .Returns(GetTestParameters().FirstOrDefault(p => p.Id == testParamId));
            var controller = new ParametersController(mock.Object);

            // Act
            var result = controller.Details(testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Parameters>(viewResult.ViewData.Model);
            Assert.Equal("1", model.Name);
            Assert.Equal(testParamId, model.Id);
        }
    }
}
