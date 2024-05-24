using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfoodApp.Modelos;

namespace EfoodApp.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITarjetaProcesadorPagoRepositorio : IRepositorio<TarjetaProcesadorPago>
    {
        void Actualizar(TarjetaProcesadorPago tarjetaProcesadorPago);

    }
}
