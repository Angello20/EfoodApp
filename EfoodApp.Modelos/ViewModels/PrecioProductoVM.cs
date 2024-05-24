    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace EfoodApp.Modelos.ViewModels
    {
        public class PrecioProductoVM
        {
            public Producto Producto { get; set; }
            public PrecioProducto PrecioProducto { get; set; }
            public IEnumerable<PrecioProducto> PreciosProducto { get; set; }
            public IEnumerable<SelectListItem> TiposPrecio { get; set; }

            public int ProductoId { get; set; } 
        }

    }
