using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Areas.Admin.Controllers;
using EfoodApp.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EFoodTests
{
    [TestClass]
    public class TiqueteControllerTests
    {
        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var controller = new TiqueteController(mockUnidadTrabajo.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task Upsert_GET_ExistingId_ReturnsViewWithExistingTiquete()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var existingTiquete = new Tiquete { Id = 6 };
            mockUnidadTrabajo.Setup(repo => repo.Tiquete.Obtener(It.IsAny<int>())).ReturnsAsync(existingTiquete);

            var controller = new TiqueteController(mockUnidadTrabajo.Object);

            // Act
            var result = await controller.Upsert(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(Tiquete));
            var tiquete = (Tiquete)result.Model;
            Assert.AreSame(existingTiquete, tiquete);
        }


    }
}
