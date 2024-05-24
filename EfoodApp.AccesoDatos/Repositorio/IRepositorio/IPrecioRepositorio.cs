using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfoodApp.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EfoodApp.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPrecioRepositorio : IRepositorio<Precio>
    {
        void Actualizar(Precio precio);
        Task<IEnumerable<SelectListItem>> ObtenerTodosDropDownLista();
    }
}
