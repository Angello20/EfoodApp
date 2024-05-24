using EfoodApp.AccesoDatos.Data;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ILineaRepositorio Linea { get; private set; }
        public IPrecioRepositorio Precio { get; private set; }
        public ITarjetaRepositorio Tarjeta { get; private set; }
        public IProcesadorPagoRepositorio ProcesadorPago { get; private set; }
        public ITarjetaProcesadorPagoRepositorio TarjetaProcesadorPago { get; private set; }
        public ITiqueteRepositorio Tiquete { get; private set; }
        public IProductoRepositorio Producto { get; private set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }
        public IPrecioProductoRepositorio PrecioProducto { get; private set; }
        public ICarroCompraRepositorio CarroCompra { get; private set; }
        public IPedidoRepositorio Pedido { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Linea = new LineaRepositorio(_db);
            Precio = new PrecioRepositorio(_db);
            Tarjeta = new TarjetaRepositorio(_db);
            Producto = new ProductoRepositorio(_db);
            UsuarioAplicacion = new UsuarioAplicacionRepositorio(_db);
            PrecioProducto = new PrecioProductoRepositorio(_db);
            CarroCompra = new CarroCompraRepositorio(_db);
            ProcesadorPago = new ProcesadorPagoRepositorio(_db);
            TarjetaProcesadorPago = new TarjetaProcesadorPagoRepositorio(_db);
            Tiquete = new TiqueteRepositorio(_db);
            Pedido = new PedidoRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
