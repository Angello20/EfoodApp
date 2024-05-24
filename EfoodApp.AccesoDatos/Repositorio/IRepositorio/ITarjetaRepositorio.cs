using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfoodApp.Modelos;

namespace EfoodApp.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITarjetaRepositorio : IRepositorio<Tarjeta>
    {
        void Actualizar(Tarjeta tarjeta);
    }
}
