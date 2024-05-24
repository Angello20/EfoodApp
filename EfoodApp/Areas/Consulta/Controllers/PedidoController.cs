using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EfoodApp.Areas.Consulta.Controllers
{
    [Area("Consulta")]
    [Authorize(Roles = DS.Role_Cliente + "," + DS.Role_Consulta + "," + DS.Role_Admin)]
    public class PedidoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor de PedidoController.
        // Inyecta la dependencia IUnidadTrabajo para interactuar con la base de datos.
        public PedidoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API

        // Método API GET para obtener todos los pedidos del usuario autenticado.
        // Opcionalmente filtra los pedidos por su estado.
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(string estado = "")
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Verifica si el usuario está autenticado.
            if (claim != null)
            {
                var usuarioAplicacionId = claim.Value;
                IEnumerable<Pedido> pedidos;

                // Filtra los pedidos basados en el estado si se proporciona.
                // De lo contrario, obtiene todos los pedidos del usuario.
                if (string.IsNullOrEmpty(estado) || estado == "Todos")
                {
                    pedidos = await _unidadTrabajo.Pedido.ObtenerTodos(p => p.UsuarioAplicacionId == usuarioAplicacionId);
                }
                else
                {
                    pedidos = await _unidadTrabajo.Pedido.ObtenerTodos(p => p.UsuarioAplicacionId == usuarioAplicacionId && p.EstadoOrden == estado);
                }

                // Formatea los pedidos para la respuesta JSON.
                var pedidosList = pedidos.Select(p => new
                {
                    p.Id,
                    p.TransaccionId,
                    FechaCreacion = p.FechaCreacion.ToString("yyyy-MM-dd"),
                    p.TotalOrden,
                    p.EstadoOrden
                });

                return Json(new { data = pedidosList });
            }
            else
            {
                // Retorna una lista vacía si el usuario no está autenticado.
                return Json(new { data = new List<Pedido>() });
            }
        }



        // Método API POST para eliminar un pedido específico.
        // Requiere el id del pedido a eliminar.
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var pedidoDb = await _unidadTrabajo.Pedido.Obtener(id);
            // Verifica si el pedido existe.
            if (pedidoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Pedido" });
            }
            // Elimina el pedido y guarda los cambios en la base de datos.
            _unidadTrabajo.Pedido.Remover(pedidoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Pedido borrado exitosamente" });
        }


        #endregion




    }
}
