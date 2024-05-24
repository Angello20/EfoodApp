using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.Modelos;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;
using EfoodApp.Modelos.ViewModels;


namespace EfoodApp.Areas.Consulta.Controllers
{
    // Controlador ProductoController en el área de Consulta.
    // Requiere que el usuario tenga uno de los roles: Cliente, Consulta o Admin.
    [Area("Consulta")]
    [Authorize(Roles = DS.Role_Cliente + "," + DS.Role_Consulta + "," + DS.Role_Admin)]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor que inyecta la dependencia IUnidadTrabajo para interactuar con la base de datos.
        public ProductoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.LineaLista = await _unidadTrabajo.Linea.ObtenerTodos();
            return View();
        }




        // Método GET para obtener los productos con sus precios.
        // Opcionalmente filtra por línea de producto si se proporciona un id de línea
        [HttpGet]
        public async Task<IActionResult> ObtenerProductosConPrecios(int? lineaId = null)
        {

            List<PrecioProductoVM> modelos = new List<PrecioProductoVM>();


            // Filtra los productos por línea de producto si se proporciona un id de línea.
            // De lo contrario, obtiene todos los productos.
            var productosQuery = lineaId.HasValue ?
                _unidadTrabajo.Producto.ObtenerTodos(p => p.LineaId == lineaId.Value) :
                _unidadTrabajo.Producto.ObtenerTodos();

            var productos = await productosQuery;


            // Itera sobre cada producto para obtener sus precios y crea un ViewModel por producto.
            foreach (var producto in productos)
            {
                // Obtener los precios asociados a cada producto
                var preciosProducto = await _unidadTrabajo.PrecioProducto.ObtenerTodos(
                    p => p.ProductoId == producto.Id,
                    incluirPropiedades: "Precio");


                var modelo = new PrecioProductoVM
                {
                    Producto = producto,
                    PreciosProducto = preciosProducto,
                    TiposPrecio = await _unidadTrabajo.Precio.ObtenerTodosDropDownLista()
                };

                
                modelos.Add(modelo);
            }

            return Json(modelos); 
        }





    }



}
