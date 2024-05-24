using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TarjetaControllerTests
    {
        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var controller = new TarjetaController(mockUnidadTrabajo.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Upsert_GET_ExistingId_ReturnsViewWithExistingTarjeta()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var existingTarjeta = new Tarjeta { Id = 6 };
            mockUnidadTrabajo.Setup(repo => repo.Tarjeta.Obtener(It.IsAny<int>())).ReturnsAsync(existingTarjeta);

            var controller = new TarjetaController(mockUnidadTrabajo.Object);

            // Act
            var result = await controller.Upsert(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(existingTarjeta, result.Model);
        }


    }
}
