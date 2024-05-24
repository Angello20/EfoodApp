using EfoodApp.AccesoDatos.Data;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Seguridad)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        // Constructor del controlador UsuarioController.
        // Inicializa la unidad de trabajo y la base de datos para operaciones relacionadas con usuarios.
        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API

        // Método GET para la API.
        // Obtiene todos los usuarios y sus roles asociados, devolviendo los datos en formato JSON.
        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var usuarioLista = await _unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = usuarioLista });
        }

        // Método POST para la API.
        // Cambia el estado de bloqueo de un usuario especificado por su ID.
        // Retorna un mensaje JSON de éxito o error dependiendo del resultado de la operación.
        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                // Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación Exitosa" });
        }


        #endregion
    }
}
