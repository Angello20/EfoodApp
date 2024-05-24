using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos.ViewModels
{
    public class ConfirmacionFinalVM
    {
        public Pedido Pedido { get; set; }
        public string UltimosCuatroDigitosTarjeta { get; set; }
        public string TipoDePago { get; set; } 
    }


}
