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
    public class TarjetaProcesadorPagoRepositorio : Repositorio<TarjetaProcesadorPago>, ITarjetaProcesadorPagoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TarjetaProcesadorPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TarjetaProcesadorPago tarjetaProcesadorPago)
        {
            var tarjetaProcesadorPagoBD = _db.TarjetasProcesadorPago.FirstOrDefault(b => b.Id == tarjetaProcesadorPago.Id);
            if (tarjetaProcesadorPagoBD != null)
            {

                tarjetaProcesadorPagoBD.TarjetaId = tarjetaProcesadorPago.TarjetaId;
                tarjetaProcesadorPagoBD.ProcesadorPagoId = tarjetaProcesadorPago.ProcesadorPagoId;
                
                _db.SaveChanges();
            }
        }


    }
}
