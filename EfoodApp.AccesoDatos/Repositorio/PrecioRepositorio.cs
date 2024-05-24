using EfoodApp.AccesoDatos.Data;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio
{
    public class PrecioRepositorio : Repositorio<Precio>, IPrecioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public PrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Precio precio)
        {
            var precioBD = _db.Lineas.FirstOrDefault(b => b.Id == precio.Id);
            if (precioBD != null)
            {
                precioBD.Codigo = precio.Codigo;
                precioBD.Descripcion = precio.Descripcion;
                _db.SaveChanges();
            }
        }


        public async Task<IEnumerable<SelectListItem>> ObtenerTodosDropDownLista()
        {
            return await _db.Precios.Select(p => new SelectListItem()
            {
                Text = p.Descripcion,
                Value = p.Id.ToString()
            }).ToListAsync();
        }


    }
}
