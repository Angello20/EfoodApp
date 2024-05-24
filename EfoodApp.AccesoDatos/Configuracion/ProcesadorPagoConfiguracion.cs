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
    public class ProcesadorPagoConfiguracion : IEntityTypeConfiguration<ProcesadorPago>
    {
        public void Configure(EntityTypeBuilder<ProcesadorPago> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Codigo).IsRequired();
            builder.Property(x => x.Procesador).IsRequired();
            builder.Property(x => x.Nombre).IsRequired();
            builder.Property(x => x.Tipo).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Verificacion).IsRequired();

        }
    }



}
