using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EfoodApp.Modelos;
using System.Reflection;
using EfoodApp.AccesoDatos.Configuracion;

namespace EfoodApp.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Linea> Lineas { get; set; }
        public DbSet<Precio> Precios { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<ProcesadorPago> ProcesadoresPago { get; set; }
        public DbSet<TarjetaProcesadorPago> TarjetasProcesadorPago { get; set; }
        public DbSet<Tiquete> Tiquetes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        public DbSet<PrecioProducto> PreciosProductos { get; set; }
        public DbSet<CarroCompra> CarroCompras { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
