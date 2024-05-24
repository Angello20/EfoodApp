using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Modelos.ViewModels;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EfoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor del controlador ProductoController.
        // Inicializa la unidad de trabajo para operaciones con productos y el entorno web para manejo de archivos.
        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear un nuevo producto.
        // Si id no es null, carga un producto existente para su edición.
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Linea"),
            };

            if (id == null)
            {
                // Crear nuevo Producto
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }




        }


        // Método POST para Upsert.
        // Crea o actualiza un producto y maneja la carga de imágenes.
        // Redirige al Index si la operación es exitosa, de lo contrario, retorna la misma vista con un mensaje de error.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    // Crear
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoVM.Producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    // Actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (files.Count > 0) // Si se carga una nueva Imagen para el producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        // Borrar la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImagenUrl = fileName + extension;


                    }
                    // Caso contrario no se carga una nueva imagen
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }

                TempData[DS.Exitosa] = "Transaccion Exitosa!";
                await _unidadTrabajo.Guardar();
                return View("Index");

            }

            // If not valid
            productoVM.LineaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownLista("Linea");
            return View(productoVM);

        }



        // Método POST para agregar un nuevo precio a un producto.
        // Si el modelo es válido, agrega el nuevo precio y redirige a la vista de precios del producto.
        // Si el modelo no es válido, recarga la vista con la información necesaria.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarPrecioProducto(PrecioProductoVM precioProductoVM)
        {
            if (ModelState.IsValid)
            {
                var nuevoPrecio = new PrecioProducto
                {
                    ProductoId = precioProductoVM.PrecioProducto.ProductoId,
                    PrecioId = precioProductoVM.PrecioProducto.PrecioId,
                    Monto = precioProductoVM.PrecioProducto.Monto
                };

                await _unidadTrabajo.PrecioProducto.Agregar(nuevoPrecio);
                await _unidadTrabajo.Guardar();
                // Redirige a la vista de precios del producto con el ID correspondiente
                return RedirectToAction("ObtenerPrecios", new { id = nuevoPrecio.ProductoId });
            }

            // Si el modelo no es válido, recargar la vista con la información necesaria
            precioProductoVM.TiposPrecio = await _unidadTrabajo.Precio.ObtenerTodosDropDownLista();
            return View("ObtenerPrecios", precioProductoVM);
        }






        // Método GET para editar el precio de un producto.
        // Carga la información del precio a editar y muestra la vista correspondiente.
        [HttpGet("Admin/Producto/EditarPrecioProducto/{id}")]
        public async Task<IActionResult> EditarPrecioProducto(int id)
        {
            var precioProducto = await _unidadTrabajo.PrecioProducto.ObtenerPrimero(p => p.Id == id, incluirPropiedades: "Precio");
            if (precioProducto == null)
            {
                return NotFound();
            }

            var modelo = new PrecioProductoVM
            {
                PrecioProducto = precioProducto,
                ProductoId = precioProducto.ProductoId
            };

            return View(modelo);
        }


        // Método POST para actualizar el precio de un producto.
        // Si el modelo es válido, actualiza el precio y redirige a la vista de precios del producto.
        // Si el modelo no es válido, recarga la vista con el modelo actual.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPrecioProducto(PrecioProductoVM modelo)
        {
            if (ModelState.IsValid)
            {
                var precioProducto = await _unidadTrabajo.PrecioProducto.Obtener(modelo.PrecioProducto.Id);
                if (precioProducto != null)
                {
                    precioProducto.Monto = modelo.PrecioProducto.Monto;
                    _unidadTrabajo.PrecioProducto.Actualizar(precioProducto);
                    await _unidadTrabajo.Guardar();
                    TempData["Exitosa"] = "Precio actualizado con éxito!";
                    string url = Url.Action("ObtenerPrecios", "Producto", new { area = "Admin", id = modelo.PrecioProducto.ProductoId });
                    return Redirect(url);
                }
            }

            // Si hay un problema, simplemente renderiza la vista de nuevo con el modelo actual
            return View(modelo);
        }











        #region API

        // Método GET para la API.
        // Obtiene todos los productos y los retorna en formato JSON, incluyendo sus líneas asociadas.
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Linea");
            return Json(new { data = todos });
        }

        // Método GET para obtener los precios de un producto específico.
        // Carga los precios asociados al producto y muestra la vista correspondiente.
        [HttpGet("Admin/Producto/AgregarPrecios/{id}")]
        public async Task<IActionResult> ObtenerPrecios(int id)
        {
            var producto = await _unidadTrabajo.Producto.Obtener(id);
            if (producto == null)
            {
                return NotFound();
            }

            var preciosProducto = await _unidadTrabajo.PrecioProducto.ObtenerTodos(p => p.ProductoId == id, incluirPropiedades: "Precio");
            var modelo = new PrecioProductoVM
            {
                Producto = producto,
                PreciosProducto = preciosProducto,
                TiposPrecio = await _unidadTrabajo.Precio.ObtenerTodosDropDownLista()
            };

            return View(modelo);
        }






        // Método POST para eliminar un producto.
        // Elimina el producto y su imagen asociada del sistema y retorna un mensaje JSON de éxito o error.
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Producto" });
            }

            // Remover imagen
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, productoDb.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado exitosamente" });
        }

        // Método GET para validar el código de un producto.
        // Verifica si el código proporcionado ya existe en la base de datos y retorna un valor booleano en formato JSON.
        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();

            Console.WriteLine($"ValidarCodigo llamado con código: {codigo} y ID: {id}"); 

            if (id == 0)
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim() && b.Id != id);
            }

            Console.WriteLine($"Código ya existe: {valor}"); 
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }


        // Método POST para eliminar un precio de producto.
        // Elimina el precio del producto especificado y retorna un mensaje JSON de éxito o error.
        [HttpPost]
        public async Task<IActionResult> DeletePrecio(int id)
        {
            var precioProducto = await _unidadTrabajo.PrecioProducto.Obtener(id);
            if (precioProducto == null)
            {
                return Json(new { success = false, message = "Error al encontrar el precio" });
            }

            _unidadTrabajo.PrecioProducto.Remover(precioProducto);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Precio eliminado con éxito" });
        }


        #endregion



    }
}
