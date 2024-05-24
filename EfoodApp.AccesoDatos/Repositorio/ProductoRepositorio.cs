using EfoodApp.AccesoDatos.Data;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (productoBD != null)
            {
                if (producto.ImagenUrl != null)
                {
                    productoBD.ImagenUrl = producto.ImagenUrl;
                }

                productoBD.Codigo = producto.Codigo;
                productoBD.Descripcion = producto.Descripcion;
                productoBD.LineaId = producto.LineaId;
                productoBD.Contenido = producto.Contenido;
                

                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownLista(string obj)
        {
            if (obj == "Linea")
            {
                return _db.Lineas.Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }


    }
}
