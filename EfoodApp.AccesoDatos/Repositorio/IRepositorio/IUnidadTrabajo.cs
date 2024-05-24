using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        ILineaRepositorio Linea { get; }
        IPrecioRepositorio Precio { get; }
        ITarjetaRepositorio Tarjeta { get; }
        IProcesadorPagoRepositorio ProcesadorPago { get; }
        ITarjetaProcesadorPagoRepositorio TarjetaProcesadorPago { get; }
        ITiqueteRepositorio Tiquete { get; }
        IProductoRepositorio Producto { get; }
        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
        IPrecioProductoRepositorio PrecioProducto { get; }
        ICarroCompraRepositorio CarroCompra { get; }
        IPedidoRepositorio Pedido { get; }
        Task Guardar();
    }
}
