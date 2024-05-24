using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.Areas.Admin.Controllers;
using EfoodApp.Modelos;
using Moq;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;

namespace EFoodTests
{
    [TestClass]
    public class LineaControllerTests
    {
        [TestMethod]
        public async Task Upsert_GET_ReturnsViewWithExistingLinea()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var existingLinea = new Linea { Id = 6 };
            mockUnidadTrabajo.Setup(repo => repo.Linea.Obtener(It.IsAny<int>())).ReturnsAsync(existingLinea);

            var controller = new LineaController(mockUnidadTrabajo.Object);

            // Act
            var result = await controller.Upsert(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(existingLinea, result.Model);
        }

    }
}
