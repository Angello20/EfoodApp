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
    public class TarjetaRepositorio : Repositorio<Tarjeta>, ITarjetaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tarjeta tarjeta)
        {
            var tarjetaBD = _db.Tarjetas.FirstOrDefault(b => b.Id == tarjeta.Id);
            if (tarjetaBD != null)
            {
                tarjetaBD.Codigo = tarjeta.Codigo;
                tarjetaBD.Descripcion = tarjeta.Descripcion;
                _db.SaveChanges();
            }
        }
    }
}
