using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class PrecioProducto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [Required]
        public int PrecioId { get; set; }

        [ForeignKey("PrecioId")]
        public Precio Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Monto { get; set; }
    }
}
