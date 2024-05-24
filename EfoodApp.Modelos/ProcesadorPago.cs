using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class ProcesadorPago
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Código es Requerido")]
        [MaxLength(20, ErrorMessage = "Código debe ser Máximo 20 Caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Procesador es Requerido")]
        [MaxLength(60, ErrorMessage = "Procesador debe ser Máximo 60 Caracteres")]
        public string Procesador { get; set; }

        [Required(ErrorMessage = "Nombre de opción de pago es Requerido")]
        [MaxLength(60, ErrorMessage = "Nombre de opción de pago debe ser Máximo 60 Caracteres")]
        public string Nombre { get; set; }

        [Required]
        public string Tipo { get; set; }


        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }


        [Required(ErrorMessage = "Verificacion es Requerido")]
        public bool Verificacion { get; set; }

        [Required(ErrorMessage = "Verificacion es Requerido")]
        public string Metodo { get; set; }
    }
}
