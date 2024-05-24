using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos.ViewModels
{
    public class ProcesadorPagoVM
    {
        public ProcesadorPago ProcesadorPago { get; set; }

        public IEnumerable<int> TarjetasAsignadasIds { get; set; }
        public IEnumerable<SelectListItem> TarjetasDisponibles { get; set; }

        public IEnumerable<SelectListItem> TarjetasAsignadas { get; set; }

        public IEnumerable<SelectListItem> TarjetasLista { get; set; }  
    }
}
