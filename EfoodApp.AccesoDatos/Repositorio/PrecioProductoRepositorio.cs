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
    public class PrecioProductoRepositorio : Repositorio<PrecioProducto>, IPrecioProductoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public PrecioProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PrecioProducto precioProducto)
        {
            var precioProductoBD = _db.PreciosProductos.FirstOrDefault(b => b.Id == precioProducto.Id);
            if (precioProductoBD != null)
            {
                precioProductoBD.ProductoId = precioProducto.ProductoId;
                precioProductoBD.PrecioId = precioProducto.PrecioId;
                precioProductoBD.Monto = precioProducto.Monto;
                    

                _db.SaveChanges();
            }
        }


    }
}
