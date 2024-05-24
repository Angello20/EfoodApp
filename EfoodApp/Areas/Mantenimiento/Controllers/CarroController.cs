using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Modelos.ViewModels;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EfoodApp.Areas.Mantenimiento.Controllers
{
    [Area("Mantenimiento")]
    public class CarroController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        // Constructor del controlador CarroController.
        // Inicializa la unidad de trabajo para operaciones de base de datos relacionadas con el carro de compras.
        public CarroController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Pedido = new Modelos.Pedido();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                u => u.UsuarioAplicacionId == claim.Value, incluirPropiedades: "Producto");

            carroCompraVM.Pedido.TotalOrden = 0;
            carroCompraVM.Pedido.UsuarioAplicacionId = claim.Value;

            foreach (var item in carroCompraVM.CarroCompraLista)
            {
                var precioProducto = await _unidadTrabajo.PrecioProducto.ObtenerPrimero(
                    pp => pp.Id == item.PrecioProductoId);

                item.Precio = Convert.ToDouble(precioProducto.Monto);
                carroCompraVM.Pedido.TotalOrden += item.Precio * item.Cantidad;
            }

            return View(carroCompraVM);
        }

        // Método GET para incrementar la cantidad de un producto en el carro de compras.
        // Busca el producto en el carro de compras por su ID y aumenta su cantidad en uno.
        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            carroCompras.Cantidad += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        // Método GET para disminuir la cantidad de un producto en el carro de compras.
        // Si la cantidad es uno, elimina el producto del carro. De lo contrario, reduce la cantidad en uno.
        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);

            if (carroCompras.Cantidad == 1)
            {
                Console.WriteLine($"La cantidad del carro con ID {carroId} es 1, se procederá a remover");

                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();

                // Disminuir directamente la cantidad en la sesión
                var numeroProductosEnSesion = HttpContext.Session.GetInt32(DS.ssCarroCompras) ?? 0;
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductosEnSesion - 1);

                Console.WriteLine($"Producto removido, nueva cantidad en la sesión: {HttpContext.Session.GetInt32(DS.ssCarroCompras)}");
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unidadTrabajo.Guardar();
                Console.WriteLine($"Nueva cantidad después de disminuir: {carroCompras.Cantidad}");
            }

            return RedirectToAction("Index");
        }

        // Método GET para eliminar un producto del carro de compras.
        // Elimina el producto del carro de compras del usuario y actualiza la sesión con el nuevo conteo de productos.
        public async Task<IActionResult> remover(int carroId)
            {
                // Remueve el Registro del Carro de Compras y Actualiza la sesion
                var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                    c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);

                var numeroProductos = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);

                return RedirectToAction("Index");
            }

        // Método GET para preparar la realización del pedido.
        // Obtiene los productos en el carro de compras y calcula el total, preparando la vista para proceder con el pedido.
        public async Task<IActionResult> Proceder()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM
            {
                Pedido = new Modelos.Pedido(),
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                    c => c.UsuarioAplicacionId == claim.Value)
            };

            carroCompraVM.Pedido.TotalOrden = 0;
            carroCompraVM.Pedido.UsuarioAplicacion = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == claim.Value);

            foreach (var item in carroCompraVM.CarroCompraLista)
            {
                
                item.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == item.ProductoId);
                item.PrecioProducto = await _unidadTrabajo.PrecioProducto.ObtenerPrimero(pp => pp.Id == item.PrecioProductoId);

                item.Precio = Convert.ToDouble(item.PrecioProducto.Monto);
                carroCompraVM.Pedido.TotalOrden += item.Precio * item.Cantidad;
            }

            carroCompraVM.Pedido.NombresCliente = carroCompraVM.Pedido.UsuarioAplicacion.Nombres + " " + carroCompraVM.Pedido.UsuarioAplicacion.Apellidos;
            carroCompraVM.Pedido.Telefono = carroCompraVM.Pedido.UsuarioAplicacion.PhoneNumber;
            carroCompraVM.Pedido.Direccion = carroCompraVM.Pedido.UsuarioAplicacion.Direccion;

            carroCompraVM.CalcularTotales();
    
            return View(carroCompraVM);
        }

        // Método POST para aplicar un tiquete de descuento al pedido.
        // Verifica la validez del tiquete de descuento y, si es válido, aplica el descuento al total del pedido.
        [HttpPost]
        public async Task<IActionResult> AplicarTiquete(string codigoTiquete, CarroCompraVM model)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var usuarioAplicacionId = claim.Value;

                
                var tiquete = await _unidadTrabajo.Tiquete.ObtenerPrimero(t => t.Codigo == codigoTiquete && t.Disponibles > 0);
                if (tiquete != null)
                {
                    
                    var descuentoPorcentaje = tiquete.Descuento / 100.0;  // Convertir a porcentaje
                    Console.WriteLine($"Descuento a aplicar: {descuentoPorcentaje * 100}%");

                    // Repoblar la lista de productos en el carrito y recalcular el total sin descuento
                    model.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                        c => c.UsuarioAplicacionId == usuarioAplicacionId, incluirPropiedades: "Producto,PrecioProducto");
                    model.Pedido.TotalOrden = model.CarroCompraLista.Sum(item => item.Cantidad * Convert.ToDouble(item.PrecioProducto.Monto));

                    // Calcular el total con descuento
                    var descuentoValor = model.Pedido.TotalOrden * descuentoPorcentaje;
                    model.Pedido.TotalOrden -= descuentoValor;
                    Console.WriteLine($"Total con descuento: {model.Pedido.TotalOrden}");

                    
                    tiquete.Disponibles -= 1;

                    model.Descuento = 1 - (tiquete.Descuento / 100.0); 
                    model.CalcularTotales(); // Recalcular los totales con el nuevo descuento
                    _unidadTrabajo.Tiquete.Actualizar(tiquete);
                    await _unidadTrabajo.Guardar();



                    TempData["MensajeExito"] = $"Descuento de {tiquete.Descuento}% aplicado exitosamente. Ahorro total: {descuentoValor}";
                }
                else
                {
                    TempData["MensajeError"] = "Código de tiquete inválido o sin disponibilidad.";
                }
                // Repoblar la lista de productos en el carrito y recalcular el precio de cada uno
                model.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                    c => c.UsuarioAplicacionId == usuarioAplicacionId, incluirPropiedades: "Producto,PrecioProducto");

                foreach (var item in model.CarroCompraLista)
                {
                    item.Precio = Convert.ToDouble(item.PrecioProducto.Monto); 
                }
                // Repoblar los datos del usuario de la aplicación
                model.Pedido.UsuarioAplicacion = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == usuarioAplicacionId);
                model.Pedido.NombresCliente = model.Pedido.UsuarioAplicacion.Nombres + " " + model.Pedido.UsuarioAplicacion.Apellidos;
                model.Pedido.Telefono = model.Pedido.UsuarioAplicacion.PhoneNumber;
                model.Pedido.Direccion = model.Pedido.UsuarioAplicacion.Direccion;
            }

            // Regresar a la vista con el modelo actualizado
            return View("Proceder", model);
        }

        // Método POST para procesar el pedido actual.
        // Calcula el subtotal y total del pedido basándose en el carro de compras y redirige a la página de selección de forma de pago.
        [HttpPost]
        public async Task<IActionResult> RealizarPedido([FromForm] CarroCompraVM model)
        {
            Console.WriteLine("Inicio del método RealizarPedido");

            
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var usuarioAplicacionId = claim.Value;
                Console.WriteLine($"UsuarioAplicacionId: {usuarioAplicacionId}");

                
                model.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                    c => c.UsuarioAplicacionId == usuarioAplicacionId, incluirPropiedades: "Producto,PrecioProducto");

                
                if (!model.CarroCompraLista.Any())
                {
                    Console.WriteLine("La lista de CarroCompra está vacía.");
                }

                
                model.Subtotal = model.CarroCompraLista.Sum(c => c.Cantidad * Convert.ToDouble(c.PrecioProducto.Monto));
                Console.WriteLine($"Subtotal calculado: {model.Subtotal}");

                
                model.Descuento = (model.Descuento > 0) ? model.Descuento : 0;
                Console.WriteLine($"Descuento aplicado: {model.Descuento}");

                
                model.Total = model.Subtotal - (model.Subtotal * model.Descuento);
                Console.WriteLine($"Total calculado: {model.Total}");
            }
            else
            {
                Console.WriteLine("No se encontró el claim del usuario.");
            }

            
            Console.WriteLine("Redirigiendo a la vista FormaDePago");
            TempData["CarroCompraVM"] = JsonConvert.SerializeObject(model);
            return RedirectToAction("FormaDePago", model);
        }



        // Método POST para procesar la forma de pago seleccionada para el pedido.
        // Dependiendo de la forma de pago elegida, redirige a la vista correspondiente para completar la transacción.
        [HttpPost]
        public IActionResult ProcesarPago(CarroCompraVM modelo)
        {
            if (string.IsNullOrEmpty(modelo.tipoDePago))
            {
                
                ModelState.AddModelError("tipoDePago", "Debe seleccionar un método de pago.");

                
                return View("FormaDePago", modelo);
            }

            // Continúa con el procesamiento según el tipo de pago seleccionado
            switch (modelo.tipoDePago)
            {
                case "Efectivo":
                    
                    TempData["TipoDePago"] = modelo.tipoDePago;
                    return RedirectToAction("ConfirmacionFinal");
                case "TarjetaCreditoDebito":
                    
                    TempData["TipoDePago"] = modelo.tipoDePago;
                    return View("VistaTarjetaCreditoDebito", modelo);
                case "ChequeElectronico":
                    
                    TempData["TipoDePago"] = modelo.tipoDePago;
                    return View("VistaChequeElectronico", modelo);
                default:
                    
                    return View("Error");
            }
        }

        // Método POST para procesar el pago con tarjeta de crédito o débito.
        // Realiza la lógica necesaria para procesar el pago con tarjeta y redirige a la vista de confirmación final del pedido.
        [HttpPost]
        public IActionResult ProcesarPagoTarjeta(CarroCompraVM model, string NumeroTarjeta)
        {
            
            var ultimosCuatroDigitos = NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);

            TempData["UltimosCuatroDigitosTarjeta"] = NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);


            return RedirectToAction("ConfirmacionFinal", new { id = model.Pedido.Id });
        }





        // Método GET para mostrar la vista de selección de forma de pago.
        // Presenta al usuario las opciones disponibles para realizar el pago de su pedido.
        [HttpGet]
        public IActionResult FormaDePago()
        {
            var model = TempData["CarroCompraVM"] != null
                        ? JsonConvert.DeserializeObject<CarroCompraVM>((string)TempData["CarroCompraVM"])
                        : new CarroCompraVM();
            return View(model);
        }

        // Método GET para mostrar la vista de confirmación final del pedido.
        // Presenta los detalles finales del pedido, incluyendo información de pago y envío.
        [HttpGet]
        public async Task<IActionResult> ConfirmacionFinal()
        {
            var tipoDePago = TempData["TipoDePago"] as string;

            Console.WriteLine(tipoDePago);
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                
            }

            var usuarioAplicacionId = claim.Value;
            var usuarioAplicacion = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == usuarioAplicacionId);

            
            var carroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                c => c.UsuarioAplicacionId == usuarioAplicacionId, incluirPropiedades: "Producto,PrecioProducto");

            var totalOrden = carroCompraLista.Sum(item => item.Cantidad * Convert.ToDouble(item.PrecioProducto.Monto));

            var confirmacionVM = new ConfirmacionFinalVM
            {
                Pedido = new Pedido
                {
                    UsuarioAplicacion = usuarioAplicacion,
                    TotalOrden = totalOrden,
                },
                TipoDePago = tipoDePago
            };

            return View(confirmacionVM);
        }

        // Método POST para finalizar el proceso de pedido.
        // Guarda la información del pedido en la base de datos y realiza las operaciones necesarias para completar la transacción.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPedido()
        {

            // Obtiene la identidad del usuario actual para validar su autenticación.
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Verifica si el usuario está autenticado. Si no, redirige a la página principal.
            if (claim == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Obtiene el identificador único del usuario actual.
            var usuarioAplicacionId = claim.Value;
            Console.WriteLine($"GuardarPedido: UsuarioAplicacionId = {usuarioAplicacionId}");

            // Recupera la información del usuario desde la base de datos.
            var usuarioAplicacion = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == usuarioAplicacionId);
            if (usuarioAplicacion == null)
            {
            }

            // Obtiene la lista de productos en el carro de compras del usuario.
            var carroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                c => c.UsuarioAplicacionId == usuarioAplicacionId, incluirPropiedades: "Producto,PrecioProducto");

            // Verifica si el carro de compras está vacío.
            if (carroCompraLista == null || !carroCompraLista.Any())
            {
                TempData[DS.Error] = "Transaccion Fallida!";
                return RedirectToAction("Index", "Carro");
            }

            // Calcula el total del pedido sumando los precios de todos los productos en el carro.
            double totalOrden = carroCompraLista.Sum(item => item.Cantidad * Convert.ToDouble(item.PrecioProducto.Monto));


            // Crea una nueva instancia de Pedido con los detalles obtenidos y calculados.
            Pedido nuevoPedido = new Pedido
            {
                UsuarioAplicacionId = usuarioAplicacionId,
                FechaCreacion = DateTime.Now,
                TotalOrden = totalOrden,
                EstadoOrden = "En curso",
                TransaccionId = Guid.NewGuid().ToString(),
                Telefono = usuarioAplicacion.PhoneNumber,
                Direccion = usuarioAplicacion.Direccion,
                NombresCliente = $"{usuarioAplicacion.Nombres} {usuarioAplicacion.Apellidos}"
            };

            // Limpia el carro de compras una vez que el pedido ha sido creado.
            if (claim != null)
            {
                var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == usuarioAplicacionId);
                foreach (var itemCarro in carroCompras)
                {
                    _unidadTrabajo.CarroCompra.Remover(itemCarro);
                }

            }

            // Guarda el nuevo pedido en la base de datos.
            await _unidadTrabajo.Pedido.Agregar(nuevoPedido);
            await _unidadTrabajo.Guardar();

            
            TempData[DS.Exitosa] = "Transaccion Exitosa!";
            return RedirectToAction("Index", "Home", new { area = "" });
        }









    }
}
