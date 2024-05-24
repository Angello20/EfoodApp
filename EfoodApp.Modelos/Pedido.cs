using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } // La fecha en que se realizó la orden

        public DateTime? FechaEnvio { get; set; } // La fecha en que el repartidor recoge la orden, puede ser null hasta que se asigne un repartidor

        public DateTime? FechaLlegada { get; set; } // La fecha real en que la orden llega al cliente, puede ser null hasta que se complete la entrega

        [Required]
        public double TotalOrden { get; set; }

        [Required]
        public string EstadoOrden { get; set; }

        public string TransaccionId { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string NombresCliente { get; set; }
    }


}
