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
    public class CarroCompraConfiguracion : IEntityTypeConfiguration<CarroCompra>
    {
        public void Configure(EntityTypeBuilder<CarroCompra> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UsuarioAplicacionId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.PrecioProductoId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();

            // Relaciones
            builder.HasOne(x => x.UsuarioAplicacion)
                   .WithMany()
                   .HasForeignKey(x => x.UsuarioAplicacionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto)
                    .WithMany()
                   .HasForeignKey(x => x.ProductoId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PrecioProducto)
                    .WithMany()
                    .HasForeignKey(x => x.PrecioProductoId)
                    .OnDelete(DeleteBehavior.NoAction);

        }
    }



}
