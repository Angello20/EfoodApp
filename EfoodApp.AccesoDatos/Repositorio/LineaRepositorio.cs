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
    public class LineaRepositorio : Repositorio<Linea>, ILineaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public LineaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Linea linea)
        {
            var lineaBD = _db.Lineas.FirstOrDefault(b => b.Id == linea.Id);
            if (lineaBD != null)
            {
                lineaBD.Codigo = linea.Codigo;
                lineaBD.Descripcion = linea.Descripcion;
                _db.SaveChanges();
            }
        }
    }
}
