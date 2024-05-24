using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Código es Requerido")]
        [MaxLength(20, ErrorMessage = "Código debe ser Máximo 20 Caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descripción es Requerido")]
        [MaxLength(100, ErrorMessage = "Descripción debe ser Máximo 100 Caracteres")]
        public string Descripcion { get; set; }

        // Relación con la línea de productos
        [Required]
        public int LineaId { get; set; }

        [ForeignKey("LineaId")]
        public Linea Linea { get; set; }

        [Required(ErrorMessage = "Contenido es Requerido")]
        public string Contenido { get; set; }

        public string ImagenUrl { get; set; }

       

    }


}
