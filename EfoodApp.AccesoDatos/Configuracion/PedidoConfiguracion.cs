using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EfoodApp.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Configuracion
{
    public class PedidoConfiguracion : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();
            builder.Property(x => x.FechaCreacion).IsRequired();
            builder.Property(x => x.FechaEnvio).IsRequired(false);
            builder.Property(x => x.FechaLlegada).IsRequired(false);
            builder.Property(x => x.TotalOrden).IsRequired();
            builder.Property(x => x.EstadoOrden).IsRequired();
            builder.Property(x => x.TransaccionId).IsRequired(false);
            builder.Property(x => x.Telefono).IsRequired();
            builder.Property(x => x.Direccion).IsRequired();
            builder.Property(x => x.NombresCliente).IsRequired();

            // Relaciones
            builder.HasOne(x => x.UsuarioAplicacion)
                   .WithMany()
                   .HasForeignKey(x => x.UsuarioAplicacionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }




}
