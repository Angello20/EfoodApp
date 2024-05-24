using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class TarjetaProcesadorPago
    {
        [Key]
        public int Id { get; set; }

        public int TarjetaId { get; set; }
        [ForeignKey("TarjetaId")]
        public Tarjeta Tarjeta { get; set; }

        public int ProcesadorPagoId { get; set; }
        [ForeignKey("ProcesadorPagoId")]
        public ProcesadorPago ProcesadorPago { get; set; }
   

    }


}
