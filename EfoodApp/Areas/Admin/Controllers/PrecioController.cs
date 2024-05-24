using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.Modelos;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;

namespace EfoodApp.Areas.Admin.Controllers
{
    // Controlador PrecioController en el área Admin
    // Gestiona las operaciones CRUD para los tipos de precio en la aplicación.
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class PrecioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor del controlador.
        // Inicializa la unidad de trabajo para interactuar con la base de datos.
        public PrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear un nuevo tipo de precio
        // Si id no es null, carga un tipo de precio existente para su edición.
        public async Task<IActionResult> Upsert(int? id)
        {
            // Verifica si el modelo es válido.
            // Si el Id de la línea es 0, agrega una nueva línea, de lo contrario, actualiza la existente.
            Precio precio = new Precio();

            if (id == null)
            {
                // Crear un nuevo Precio
                return View(precio);
            }

            // Actualizamos Precio
            precio = await _unidadTrabajo.Precio.Obtener(id.GetValueOrDefault());
            if (precio == null)
            {
                return NotFound();
            }

            return View(precio);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Precio precio)
        {
            // Método POST para Upsert.
            // Si el modelo es válido, crea o actualiza un tipo de precio basado en su Id.
            // Redirige al Index si la operación es exitosa, de lo contrario, retorna la misma vista con un mensaje de error.
            if (ModelState.IsValid)
            {
                if (precio.Id == 0)
                {
                    await _unidadTrabajo.Precio.Agregar(precio);
                    TempData[DS.Exitosa] = "Precio creado Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Precio.Actualizar(precio);
                    TempData[DS.Exitosa] = "Precio actualizado Exitosamente";

                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Precio";
            return View(precio);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // Método GET para la API.
            // Obtiene todas los tipos de precio y las retorna en formato JSON.
            var todos = await _unidadTrabajo.Precio.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Método POST para la API.
            // Elimina un tipo de precio basado en su Id.
            // Retorna un mensaje de éxito o error en formato JSON.
            var precioDb = await _unidadTrabajo.Precio.Obtener(id);
            if (precioDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Precio" });
            }
            _unidadTrabajo.Precio.Remover(precioDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Precio borrado exitosamente" });
        }

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            // Método GET para validar el código de una tipo de precio.
            // Verifica si el código proporcionado ya existe en la base de datos, excluyendo el Id proporcionado si es necesario.
            // Retorna un valor booleano en formato JSON indicando si el código ya existe o no.
            bool valor = false;
            var lista = await _unidadTrabajo.Precio.ObtenerTodos();
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
