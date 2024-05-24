using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.Modelos;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Utilidades;

namespace EfoodApp.Areas.Admin.Controllers
{
    // Controlador LineaController en el área Admin
    // Gestiona las operaciones CRUD para las líneas de comida en la aplicación.
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class LineaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        // Constructor del controlador.
        // Inicializa la unidad de trabajo para interactuar con la base de datos.
        public LineaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear una nueva línea de comida.
        // Si id no es null, carga una línea existente para su edición.
        public async Task<IActionResult> Upsert(int? id)
        {
            // Verifica si el modelo es válido.
            // Si el Id de la línea es 0, agrega una nueva línea, de lo contrario, actualiza la existente.
            Linea linea = new Linea();

            if (id == null)
            {
                // Crear una nueva Linea
                return View(linea);
            }

            // Actualizamos Linea
            linea = await _unidadTrabajo.Linea.Obtener(id.GetValueOrDefault());
            if (linea == null)
            {
                return NotFound();
            }

            return View(linea);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Linea linea)
        {
            // Método POST para Upsert.
            // Si el modelo es válido, crea o actualiza la línea de comida basada en su Id.
            // Redirige al Index si la operación es exitosa, de lo contrario, retorna la misma vista con un mensaje de error.
            if (ModelState.IsValid)
            {
                if (linea.Id == 0)
                {
                    await _unidadTrabajo.Linea.Agregar(linea);
                    TempData[DS.Exitosa] = "Linea creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Linea.Actualizar(linea);
                    TempData[DS.Exitosa] = "Linea actualizada Exitosamente";

                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Linea";
            return View(linea);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // Método GET para la API.
            // Obtiene todas las líneas de comida y las retorna en formato JSON.
            var todos = await _unidadTrabajo.Linea.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            // Método POST para la API.
            // Elimina una línea de comida basada en su Id.
            // Retorna un mensaje de éxito o error en formato JSON.
            var lineaDb = await _unidadTrabajo.Linea.Obtener(id);
            if (lineaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Linea" });
            }
            _unidadTrabajo.Linea.Remover(lineaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Linea borrada exitosamente" });
        }

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id = 0)
        {
            // Método GET para validar el código de una línea de comida.
            // Verifica si el código proporcionado ya existe en la base de datos, excluyendo el Id proporcionado si es necesario.
            // Retorna un valor booleano en formato JSON indicando si el código ya existe o no.
            bool valor = false;
            var lista = await _unidadTrabajo.Linea.ObtenerTodos();
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



        #endregion



    }
}
