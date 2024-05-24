using EfoodApp.AccesoDatos.Repositorio;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using EfoodApp.Modelos.ViewModels;
using EfoodApp.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EfoodApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class ProcesadorPagoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor de ProcesadorPagoController.
        // Inicializa la unidad de trabajo y el entorno de alojamiento web.
        public ProcesadorPagoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método GET para Upsert.
        // Si id es null, prepara la vista para crear un nuevo procesador de pago.
        // Si id no es null, carga un procesador de pago existente junto con sus tarjetas asignadas para su edición.    
        public async Task<IActionResult> Upsert(int? id)
        {
            ProcesadorPagoVM procesadorPagoVM = new ProcesadorPagoVM()
            {
                ProcesadorPago = new ProcesadorPago(),
                TarjetasDisponibles = (await _unidadTrabajo.Tarjeta.ObtenerTodos())
                                    .Select(t => new SelectListItem
                                    {
                                        Text = t.Descripcion,
                                        Value = t.Id.ToString()
                                    }).ToList(),
                TarjetasAsignadas = new List<SelectListItem>() 
            };

            if (id == null)
            {
                // Crear nuevo procesador de pago
                return View(procesadorPagoVM);
            }
            else
            {
                procesadorPagoVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.Obtener(id.GetValueOrDefault());
                if (procesadorPagoVM.ProcesadorPago == null)
                {
                    return NotFound();
                }

                var tarjetasAsignadasResult = await _unidadTrabajo.TarjetaProcesadorPago.ObtenerTodos(
                    t => t.ProcesadorPagoId == id.GetValueOrDefault(),
                    incluirPropiedades: "Tarjeta"
                );

                procesadorPagoVM.TarjetasAsignadas = tarjetasAsignadasResult
                    .Select(t => new SelectListItem
                    {
                        Value = t.TarjetaId.ToString(),
                        Text = t.Tarjeta.Descripcion
                    }).ToList();

                procesadorPagoVM.TarjetasDisponibles = procesadorPagoVM.TarjetasDisponibles
                    .Where(t => !procesadorPagoVM.TarjetasAsignadas.Select(ta => ta.Value).Contains(t.Value))
                    .ToList();

                return View(procesadorPagoVM);
            }
        }







        // Método POST para Upsert.
        // Crea o actualiza un procesador de pago y gestiona las tarjetas asociadas.
        // Incluye lógica para asegurar que solo un procesador de pago esté activo por tipo.
        // Si el modelo es válido, guarda los cambios y redirige al Index.
        // Si el modelo no es válido, recarga la vista con la información necesaria.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProcesadorPagoVM procesadorPagoVM)
        {
            if (ModelState.IsValid)
            {
                if (procesadorPagoVM.ProcesadorPago.Estado) 
                {
                    await InactivarProcesadoresPorTipo(procesadorPagoVM.ProcesadorPago.Tipo, procesadorPagoVM.ProcesadorPago.Id);
                }

                procesadorPagoVM.TarjetasAsignadasIds ??= new List<int>();
                bool esNuevo = procesadorPagoVM.ProcesadorPago.Id == 0;
                if (esNuevo)
                {
                    await _unidadTrabajo.ProcesadorPago.Agregar(procesadorPagoVM.ProcesadorPago);
                    await _unidadTrabajo.Guardar(); 

                    // Crear registros en TarjetaProcesadorPago
                    foreach (var tarjetaId in procesadorPagoVM.TarjetasAsignadasIds)
                    {
                        var tarjetaProcesadorPago = new TarjetaProcesadorPago
                        {
                            ProcesadorPagoId = procesadorPagoVM.ProcesadorPago.Id,
                            TarjetaId = tarjetaId
                        };
                        await _unidadTrabajo.TarjetaProcesadorPago.Agregar(tarjetaProcesadorPago);
                    }
                    await _unidadTrabajo.Guardar();
                }
                else
                {

                    // Actualización del ProcesadorPago existente
                    _unidadTrabajo.ProcesadorPago.Actualizar(procesadorPagoVM.ProcesadorPago);

                    // Obtener las tarjetas asignadas actuales relacionadas con este ProcesadorPago
                    var tarjetasAsignadasActuales = await _unidadTrabajo.TarjetaProcesadorPago
                        .ObtenerTodos(t => t.ProcesadorPagoId == procesadorPagoVM.ProcesadorPago.Id);

                    foreach (var tarjeta in tarjetasAsignadasActuales)
                    {
                        Console.WriteLine(tarjeta.TarjetaId);
                    }
                    foreach (var tarjetaId in procesadorPagoVM.TarjetasAsignadasIds)
                    {
                        Console.WriteLine(tarjetaId);
                    }

                    // Eliminación de tarjetas ya no asignadas
                    var tarjetasParaEliminar = tarjetasAsignadasActuales.Where(t => !procesadorPagoVM.TarjetasAsignadasIds.Contains(t.TarjetaId)).ToList();

                    foreach (var tarjetaEliminar in tarjetasParaEliminar)
                    {
                        _unidadTrabajo.TarjetaProcesadorPago.Remover(tarjetaEliminar);
                    }

                    // Adición de tarjetas asignadas si hubiera
                    foreach (var tarjetaIdEnviada in procesadorPagoVM.TarjetasAsignadasIds)
                    {
                        bool coincidenciaEncontrada = tarjetasAsignadasActuales.Any(t => t.TarjetaId == tarjetaIdEnviada);
                        if (coincidenciaEncontrada)
                        {
                            // Si hay una coincidencia, simplemente continúa con el siguiente ID
                            continue; 
                        }
                        else
                        {


                            // Si no hay coincidencia, agrega la nueva tarjeta a tarjetasProcesadorPago
                            var nuevaTarjetaProcesadorPago = new TarjetaProcesadorPago
                            {
                                ProcesadorPagoId = procesadorPagoVM.ProcesadorPago.Id, // Asegúrate de que este sea el ID correcto del ProcesadorPago
                                TarjetaId = tarjetaIdEnviada
                            };
                            await _unidadTrabajo.TarjetaProcesadorPago.Agregar(nuevaTarjetaProcesadorPago);
                        }
                    }



                    await _unidadTrabajo.Guardar();
                }


                TempData["Exitosa"] = "Procesador de pago guardado con éxito";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Si el modelo no es válido, recargar la vista con la información necesaria
                procesadorPagoVM.TarjetasDisponibles = (await _unidadTrabajo.Tarjeta.ObtenerTodos())
                                        .Select(t => new SelectListItem
                                        {
                                            Text = t.Descripcion,
                                            Value = t.Id.ToString()
                                        }).ToList();
                // Carga cualquier otra propiedad necesaria para la vista aquí
                return View(procesadorPagoVM);
            }
        }

        // Método privado para inactivar otros procesadores de pago del mismo tipo.
        // Utilizado al activar un procesador de pago para asegurar que solo uno esté activo por tipo.
        private async Task InactivarProcesadoresPorTipo(string tipo, int idExcluido)
        {
            var procesadores = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.Tipo == tipo && p.Id != idExcluido);
            foreach (var procesador in procesadores)
            {
                procesador.Estado = false; // Inactiva el procesador
                _unidadTrabajo.ProcesadorPago.Actualizar(procesador);
            }
            await _unidadTrabajo.Guardar();
        }



        #region API

        // Método GET para la API.
        // Obtiene todos los procesadores de pago y los retorna en formato JSON.
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            return Json(new { data = todos });
        }


        // Método POST para eliminar un procesador de pago.
        // Elimina todas las tarjetas asociadas y luego el procesador de pago.
        // Retorna un mensaje JSON de éxito o error.
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var procesadorPago = await _unidadTrabajo.ProcesadorPago.Obtener(id);
            if (procesadorPago == null)
            {
                return Json(new { success = false, message = "Error al borrar el procesador de pago" });
            }

            // Encuentra todas las tarjetas asociadas con el procesador de pago antes de eliminarlo
            var tarjetasAsociadas = await _unidadTrabajo.TarjetaProcesadorPago
                .ObtenerTodos(filtro: t => t.ProcesadorPagoId == id);

            // Elimina todas las tarjetas asociadas
            foreach (var tarjeta in tarjetasAsociadas)
            {
                _unidadTrabajo.TarjetaProcesadorPago.Remover(tarjeta);
            }

            // Ahora elimina el procesador de pago
            _unidadTrabajo.ProcesadorPago.Remover(procesadorPago);

            // Guarda los cambios en la base de datos
            await _unidadTrabajo.Guardar();

            return Json(new { success = true, message = "Procesador de pago borrado exitosamente" });
        }


        // Método GET para validar el tipo de un procesador de pago.
        // Verifica si el tipo de procesador de pago proporcionado ya existe en la base de datos.
        // Retorna un valor booleano en formato JSON indicando si el tipo ya existe o no.
        [ActionName("ValidarTipo")]
        public async Task<IActionResult> ValidarTipo(string tipo, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Tipo.ToLower().Trim() == tipo.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Tipo.ToLower().Trim() == tipo.ToLower().Trim() && b.Id != id);
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
    