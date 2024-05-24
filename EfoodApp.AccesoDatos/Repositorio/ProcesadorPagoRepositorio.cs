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
    public class ProcesadorPagoRepositorio : Repositorio<ProcesadorPago>, IProcesadorPagoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProcesadorPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProcesadorPago procesadorPago)
        {
            var procesadorPagoBD = _db.ProcesadoresPago.FirstOrDefault(b => b.Id == procesadorPago.Id);
            if (procesadorPagoBD != null)
            {
                procesadorPagoBD.Codigo = procesadorPago.Codigo;
                procesadorPagoBD.Procesador = procesadorPago.Procesador;
                procesadorPagoBD.Nombre = procesadorPago.Nombre;
                procesadorPagoBD.Tipo = procesadorPago.Tipo;
                procesadorPagoBD.Estado = procesadorPago.Estado;
                procesadorPagoBD.Verificacion = procesadorPago.Verificacion;
                procesadorPagoBD.Metodo = procesadorPago.Metodo;
                _db.SaveChanges();
            }
        }


        public IEnumerable<SelectListItem> ObtenerTodosDropDownLista(string obj)
        {
            if (obj == "Tarjeta")
            {
                return _db.Tarjetas.Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
