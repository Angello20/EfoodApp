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
    public class PrecioProductoConfiguracion : IEntityTypeConfiguration<PrecioProducto>
    {

        public void Configure(EntityTypeBuilder<PrecioProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.PrecioId).IsRequired();
            builder.Property(x => x.Monto).IsRequired().HasColumnType("decimal(18, 2)");


            /* Relaciones */
            builder.HasOne(x => x.Precio).WithMany()
                .HasForeignKey(x => x.PrecioId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);



        }


    }
}

