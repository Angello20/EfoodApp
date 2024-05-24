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
    public class TarjetaProcesadorPagoConfiguracion : IEntityTypeConfiguration<TarjetaProcesadorPago>
    {

        public void Configure(EntityTypeBuilder<TarjetaProcesadorPago> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.TarjetaId).IsRequired(); 
            builder.Property(x => x.ProcesadorPagoId).IsRequired();

            /* Relaciones */
            builder.HasOne(x => x.Tarjeta).WithMany()
                .HasForeignKey(x => x.TarjetaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ProcesadorPago).WithMany()
                .HasForeignKey(x => x.ProcesadorPagoId)
                .OnDelete(DeleteBehavior.NoAction);

        }


    }
}
