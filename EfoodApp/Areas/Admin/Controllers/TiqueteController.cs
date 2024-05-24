using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.Modelos;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;

namespace EfoodApp.Areas.Admin.Controllers
{
    // Controlador TiqueteController en el área Admin
    // Gestiona las operaciones CRUD para los tiquetes de descuento en la aplicación.
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class TiqueteController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor del controlador.
        // Inicializa la unidad de trabajo para interactuar con la base de datos.
        public TiqueteController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear un nuevo tiquete de descuento.
        // Si id no es null, carga un tiquete de descuento existente para su edición.
        public async Task<IActionResult> Upsert(int? id)
        {
            // Verifica si el modelo es válido.
            // Si el Id de la línea es 0, agrega una nueva línea, de lo contrario, actualiza la existente.
            Tiquete tiquete = new Tiquete();

            if (id == null)
            {
                // Crear un nuevo Tiquete
                return View(tiquete);
            }

            // Actualizamos Tiquete
            tiquete = await _unidadTrabajo.Tiquete.Obtener(id.GetValueOrDefault());
            if (tiquete == null)
            {
                return NotFound();
            }

            return View(tiquete);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tiquete tiquete)
        {
            // Método POST para Upsert.
            // Si el modelo es válido, crea o actualiza un tiquete de descuento basado en su Id.
            // Redirige al Index si la operación es exitosa, de lo contrario, retorna la misma vista con un mensaje de error.
            if (ModelState.IsValid)
            {
                if (tiquete.Id == 0)
                {
                    await _unidadTrabajo.Tiquete.Agregar(tiquete);
                    TempData[DS.Exitosa] = "Tiquete creado Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tiquete.Actualizar(tiquete);
                    TempData[DS.Exitosa] = "Tiquete actualizado Exitosamente";

                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Tiquete";
            return View(tiquete);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // Método GET para la API.
            // Obtiene todas los tiquetes de descuento y las retorna en formato JSON.
            var todos = await _unidadTrabajo.Tiquete.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Método POST para la API.
            // Elimina un tipo de precio basado en su Id.
            // Retorna un mensaje de éxito o error en formato JSON.
            var tiqueteDb = await _unidadTrabajo.Tiquete.Obtener(id);
            if (tiqueteDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Tiquete" });
            }
            _unidadTrabajo.Tiquete.Remover(tiqueteDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tiquete borrado exitosamente" });
        }

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            // Método GET para validar el código de una tipo de precio.
            // Verifica si el código proporcionado ya existe en la base de datos, excluyendo el Id proporcionado si es necesario.
            // Retorna un valor booleano en formato JSON indicando si el código ya existe o no.
            bool valor = false;
            var lista = await _unidadTrabajo.Tiquete.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }



        #endregion

    }
}
