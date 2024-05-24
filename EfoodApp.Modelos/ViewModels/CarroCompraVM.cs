using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Modelos.ViewModels
{
    public class CarroCompraVM
    {
        public Producto Producto { get; set; }
        public IEnumerable<PrecioProducto> PreciosProducto { get; set; }
        public CarroCompra CarroCompra { get; set; }
        public IEnumerable<CarroCompra> CarroCompraLista { get; set; }
        public Pedido Pedido { get; set; }

        public double Subtotal { get; set; } = 0;
        public double Descuento { get; set; } = 0; // Valor predeterminado para el descuento
        public double Total { get; set; } = 0;

        public string tipoDePago { get; set; }
        // Método para calcular el subtotal y el total basado en el descuento
        public void CalcularTotales()
        {
            Subtotal = CarroCompraLista.Sum(item => item.Precio * item.Cantidad);
            Total = Subtotal * Descuento; // Si Descuento es 1, Total será igual a Subtotal
        }
    }


}
