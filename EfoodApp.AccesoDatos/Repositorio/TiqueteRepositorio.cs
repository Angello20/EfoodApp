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
    public class TiqueteRepositorio : Repositorio<Tiquete>, ITiqueteRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TiqueteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tiquete tiquete)
        {
            var tiqueteBD = _db.Tiquetes.FirstOrDefault(b => b.Id == tiquete.Id);
            if (tiqueteBD != null)
            {
                tiqueteBD.Codigo = tiquete.Codigo;
                tiqueteBD.Descripcion = tiquete.Descripcion;
                tiqueteBD.Disponibles = tiquete.Disponibles;
                tiqueteBD.Descuento = tiquete.Descuento;
                _db.SaveChanges();
            }
        }
    }
}
