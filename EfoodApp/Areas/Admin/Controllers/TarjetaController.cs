using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.Modelos;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;

namespace EfoodApp.Areas.Admin.Controllers
{
    // Controlador TarjetaController en el área Admin
    // Gestiona las operaciones CRUD para las tarjetas en la aplicación.
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class TarjetaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor del controlador.
        // Inicializa la unidad de trabajo para interactuar con la base de datos.
        public TarjetaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear una nueva tarjeta.
        // Si id no es null, carga una tarjeta existente para su edición.
        public async Task<IActionResult> Upsert(int? id)
        {
            // Verifica si el modelo es válido.
            // Si el Id de la línea es 0, agrega una nueva tarjeta, de lo contrario, actualiza la existente.
            Tarjeta tarjeta = new Tarjeta();

            if (id == null)
            {
                // Crear una nueva Tarjeta
                return View(tarjeta);
            }

            // Actualizamos Tarjeta
            tarjeta = await _unidadTrabajo.Tarjeta.Obtener(id.GetValueOrDefault());
            if (tarjeta == null)
            {
                return NotFound();
            }

            return View(tarjeta);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tarjeta tarjeta)
        {
            // Método POST para Upsert.
            // Si el modelo es válido, crea o actualiza la tarjeta basada en su Id.
            // Redirige al Index si la operación es exitosa, de lo contrario, retorna la misma vista con un mensaje de error.
            if (ModelState.IsValid)
            {
                if (tarjeta.Id == 0)
                {
                    await _unidadTrabajo.Tarjeta.Agregar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tarjeta.Actualizar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta actualizada Exitosamente";

                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Tarjeta";
            return View(tarjeta);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // Método GET para la API.
            // Obtiene todas las tarjetas y las retorna en formato JSON.
            var todos = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Método POST para la API.
            // Elimina una tarjeta basada en su Id.
            // Retorna un mensaje de éxito o error en formato JSON.
            var tarjetaDb = await _unidadTrabajo.Tarjeta.Obtener(id);
            if (tarjetaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Tarjeta" });
            }
            _unidadTrabajo.Tarjeta.Remover(tarjetaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tarjeta borrada exitosamente" });
        }

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            // Método GET para validar el código de una tarjeta.
            // Verifica si el código proporcionado ya existe en la base de datos, excluyendo el Id proporcionado si es necesario.
            // Retorna un valor booleano en formato JSON indicando si el código ya existe o no.
            bool valor = false;
            var lista = await _unidadTrabajo.Tarjeta.ObtenerTodos();
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
