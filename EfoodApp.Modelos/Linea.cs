using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class Linea
    {
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo es Requerido")]
        [MaxLength(20, ErrorMessage = "Codigo debe ser Máximo 20 Caracteres")]
        public string Codigo { get; set; }


        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Máximo 100 Caracteres")]
        public string Descripcion { get; set; }


    }
}
