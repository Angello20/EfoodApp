using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EfoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class PedidoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PedidoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(string estado = "")
        {
            IEnumerable<Pedido> pedidos;

            if (string.IsNullOrEmpty(estado) || estado == "Todos")
            {
                pedidos = await _unidadTrabajo.Pedido.ObtenerTodos();
            }
            else
            {
                pedidos = await _unidadTrabajo.Pedido.ObtenerTodos(p => p.EstadoOrden == estado);
            }

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

        #region API









        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var pedidoDb = await _unidadTrabajo.Pedido.Obtener(id);
            if (pedidoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Pedido" });
            }
            _unidadTrabajo.Pedido.Remover(pedidoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Pedido borrado exitosamente" });
        }


        [HttpPost]
        public async Task<IActionResult> CambiarEstadoPedido(int id, string nuevoEstado)
        {
            var pedidoDb = await _unidadTrabajo.Pedido.Obtener(id);
            if (pedidoDb == null)
            {
                return Json(new { success = false, message = "Pedido no encontrado." });
            }

            pedidoDb.EstadoOrden = nuevoEstado;
            _unidadTrabajo.Pedido.Actualizar(pedidoDb);
            await _unidadTrabajo.Guardar();

            return Json(new { success = true, message = "El estado del pedido se ha actualizado correctamente." });
        }



        #endregion




    }
}
