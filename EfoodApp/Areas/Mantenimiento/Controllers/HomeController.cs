using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Modelos.Especificaciones;
using EfoodApp.Modelos.ViewModels;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace EfoodApp.Areas.Mantenimiento.Controllers
{
    [Area("Mantenimiento")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        // Constructor del controlador HomeController.
        // Inicializa el logger para registro de actividades y la unidad de trabajo para operaciones de base de datos.
        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        // Método GET para la página principal.
        // Muestra una lista paginada de productos, permitiendo filtrar por búsqueda y línea de comida.
        // Gestiona la información de la sesión del usuario y el carro de compras.
        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda = "",
            string busquedaActual = "", int? lineaComidaId = null)
        {
            // Controlar sesión
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);
                var numeroProductos = carroLista.Count(); // Número de Registros
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);
            }

            //
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;


            if (pageNumber < 1) { pageNumber = 1; }

            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4
            };

            ViewBag.LineasDeComida = new SelectList(await _unidadTrabajo.Linea.ObtenerTodos(), "Id", "Descripcion");
            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if (lineaComidaId.HasValue)
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(
                    parametros, p => p.LineaId == lineaComidaId.Value &&
                    (string.IsNullOrEmpty(busqueda) || p.Descripcion.Contains(busqueda)));
            }
            else if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(
                    parametros, p => p.Descripcion.Contains(busqueda));
            }


            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; // clase css para desactivar el boton
            ViewData["Siguiente"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }

            return View(resultado);
        }

        // Método GET para mostrar los detalles de un producto.
        // Obtiene el producto y sus precios asociados para su visualización y posible agregación al carro de compras.
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            // Inicializa el ViewModel
            CarroCompraVM carroCompraVM = new CarroCompraVM();

            // Obtiene el producto y sus precios asociados
            carroCompraVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == id);
            carroCompraVM.PreciosProducto = await _unidadTrabajo.PrecioProducto.ObtenerTodos(pp => pp.ProductoId == id, incluirPropiedades: "Precio");

            if (carroCompraVM.Producto == null)
            {
                return NotFound();
            }

            // Se crea un CarroCompra vacío que se llenará en la vista
            carroCompraVM.CarroCompra = new CarroCompra()
            {
                ProductoId = id
            };

            // Retorna la vista Detalle con el ViewModel lleno
            return View("Detalle", carroCompraVM);
        }

        // Método GET para mostrar los detalles de un producto.
        // Obtiene el producto y sus precios asociados para su visualización y posible agregación al carro de compras.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Detalle(CarroCompraVM carroCompraVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Se asigna el id del usuario al CarroCompra que viene del ViewModel
            carroCompraVM.CarroCompra.UsuarioAplicacionId = claim.Value;

            // Se busca si ya existe un carro de compra para este usuario y este producto específico con el precio seleccionado
            CarroCompra carroBD = await _unidadTrabajo.CarroCompra.ObtenerPrimero(
                c => c.UsuarioAplicacionId == claim.Value &&
                c.ProductoId == carroCompraVM.CarroCompra.ProductoId &&
                c.PrecioProductoId == carroCompraVM.CarroCompra.PrecioProductoId
            );

            if (carroBD == null)
            {
                // Si no existe, se agrega uno nuevo
                await _unidadTrabajo.CarroCompra.Agregar(carroCompraVM.CarroCompra);
            }
            else
            {
                // Si ya existe, se actualiza la cantidad
                carroBD.Cantidad += carroCompraVM.CarroCompra.Cantidad;
                _unidadTrabajo.CarroCompra.Actualizar(carroBD);
            }

            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Producto agregado al Carro de Compras";

            // (Opcional) Actualizar la sesión con el nuevo conteo de productos en el carrito
            // Esta parte puede variar dependiendo de si tu aplicación maneja el conteo de productos en el carrito en la sesión.
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);
            var numeroProductos = carroLista.Count(); // Número de Registros
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);

            return RedirectToAction("Index", "Home", new { Area = "Mantenimiento" });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        // Método GET para manejo de errores.
        // Muestra una página de error personalizada con información de seguimiento.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
