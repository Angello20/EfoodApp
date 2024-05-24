using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Areas.Admin.Controllers;
using EfoodApp.Modelos;
using EfoodApp.Modelos.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EFoodTests
{
    [TestClass]
    public class ProductoControllerTests
    {
        [TestMethod]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var controller = new ProductoController(mockUnidadTrabajo.Object, mockWebHostEnvironment.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task Upsert_GET_ExistingId_ReturnsViewWithExistingProductoVM()
        {
            // Arrange
            var mockUnidadTrabajo = new Mock<IUnidadTrabajo>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var existingProducto = new Producto { Id = 6 };
            mockUnidadTrabajo.Setup(repo => repo.Producto.Obtener(It.IsAny<int>())).ReturnsAsync(existingProducto);

            var controller = new ProductoController(mockUnidadTrabajo.Object, mockWebHostEnvironment.Object);

            // Act
            var result = await controller.Upsert(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(ProductoVM));
            var productoVM = (ProductoVM)result.Model;
            Assert.AreSame(existingProducto, productoVM.Producto);
        }



    }
}