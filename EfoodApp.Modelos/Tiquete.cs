using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class Tiquete
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo es Requerido")]
        [MaxLength(20, ErrorMessage = "Codigo debe ser Máximo 20 Caracteres")]
        public string Codigo { get; set; }


        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Máximo 100 Caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Tiquetes disponibles es Requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Se debe ingresar un valor entero positivo en el campo cantidad disponible.")]
        public int Disponibles { get; set; }


        [Required(ErrorMessage = "Descuento es Requerido")]
        [Range(1, 100, ErrorMessage = "El descuento debe estar entre 1% y 100%")]
        public int Descuento { get; set; } 
    }
}
