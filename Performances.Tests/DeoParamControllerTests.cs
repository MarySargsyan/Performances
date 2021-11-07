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
    public class DeoParamControllerTests
    {
        private List<DepartmentParameters> GetTestDepParam()
        {
            var groups = new List<DepartmentParameters>
            {
                new DepartmentParameters { DepartmentId=1, ParameterId=1, mark= 5},
                new DepartmentParameters { DepartmentId=1, ParameterId=2, mark= 5},
            };
            return groups;
        }
        [Fact]
        public void AddDPReturnsARedirectAndAddParameter()
        {
            // Arrange
            var mock = new Mock<IDepParamService>();
            var controller = new DepParam(mock.Object);
            DepartmentParameters newParam = new DepartmentParameters()
            {
                ParameterId = 1, DepartmentId = 1, mark= 5          
            };

            // Act
            var result = controller.Create(newParam);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("DepartmentPage", redirectToActionResult.ActionName);
        }
        [Fact]
        public void AddDPVerifyResult()
        {
            // Arrange
            var mock = new Mock<IDepParamService>();
            var controller = new DepParam(mock.Object);
            DepartmentParameters newParam = new DepartmentParameters()
            {
                ParameterId = 1, DepartmentId = 1, mark= 5          
            };

            // Act
            var result = controller.Create(newParam);

            // Assert
            mock.Verify(r => r.Insert(newParam));
        }
        [Fact]
        public void DeleteConfirmDPVerifyResult()
        {
            // Arrange
            int testDepId = 1;
            int testParamId = 1;
            var mock = new Mock<IDepParamService>();
            mock.Setup(repo => repo.GetById(testDepId, testParamId)).
                Returns(GetTestDepParam().FirstOrDefault(p => p.DepartmentId == testDepId & p.ParameterId == testParamId));
            var controller = new DepParam(mock.Object);

            // Act
            var result = controller.DeleteConfirmed(testDepId, testParamId);


            // Assert
            mock.Verify(r => r.Delete(testDepId, testParamId));
        }
        [Fact]
        public void DeleteDPViewResult()
        {
            // Arrange
            int testDepId = 1;
            int testParamId = 1;
            var mock = new Mock<IDepParamService>();
            mock.Setup(repo => repo.GetById(testDepId, testParamId)).
                Returns(GetTestDepParam().FirstOrDefault(p => p.DepartmentId==testDepId & p.ParameterId== testParamId));
            var controller = new DepParam(mock.Object);

            // Act
            var result = controller.Delete(testDepId, testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DepartmentParameters>(viewResult.ViewData.Model);
        }
        [Fact]
        public void EditDPViewResult()
        {
            // Arrange
            int testDepId = 1;
            int testParamId = 1;
            var mock = new Mock<IDepParamService>();
            mock.Setup(repo => repo.GetById(testDepId, testParamId)).
                Returns(GetTestDepParam().FirstOrDefault(p => p.DepartmentId==testDepId & p.ParameterId== testParamId));
            var controller = new DepParam(mock.Object);

            // Act
            var result = controller.Edit(testDepId, testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DepartmentParameters>(viewResult.ViewData.Model);
        }
        [Fact]
        public void DeleteGoupReturnsDeleteGroupEqualtestGeoupId()
        {
            // Arrange
            int testDepId = 1;
            int testParamId = 1;
            var mock = new Mock<IDepParamService>();
            mock.Setup(repo => repo.GetById(testDepId, testParamId)).
                Returns(GetTestDepParam().FirstOrDefault(p => p.DepartmentId == testDepId & p.ParameterId == testParamId));
            var controller = new DepParam(mock.Object);

            // Act
            var result = controller.Delete(testDepId, testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DepartmentParameters>(viewResult.ViewData.Model);
            Assert.Equal(testDepId, model.DepartmentId);
            Assert.NotNull(model.mark);
        }
        [Fact]
        public void EditDPReturnsNotNullmark()
        {
            // Arrange
            int testDepId = 1;
            int testParamId = 1;
            var mock = new Mock<IDepParamService>();
            mock.Setup(repo => repo.GetById(testDepId, testParamId)).
                Returns(GetTestDepParam().FirstOrDefault(p => p.DepartmentId == testDepId & p.ParameterId == testParamId));
            var controller = new DepParam(mock.Object);

            // Act
            var result = controller.Edit(testDepId, testParamId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DepartmentParameters>(viewResult.ViewData.Model);
            Assert.Equal(testDepId, model.DepartmentId);
            Assert.NotNull(model.mark);
        }
    }
}
