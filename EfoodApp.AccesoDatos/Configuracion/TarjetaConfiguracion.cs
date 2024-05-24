﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EfoodApp.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EfoodApp.AccesoDatos.Configuracion
{
    public class TarjetaConfiguracion : IEntityTypeConfiguration<Tarjeta>
    {

        public void Configure(EntityTypeBuilder<Tarjeta> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Codigo).IsRequired().HasMaxLength(20); ;
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);

        }


    }
}
