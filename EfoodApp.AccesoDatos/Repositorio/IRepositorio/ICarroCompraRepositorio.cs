﻿using Microsoft.AspNetCore.Mvc.Rendering;
using EfoodApp.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarroCompraRepositorio : IRepositorio<CarroCompra>
    {
        void Actualizar(CarroCompra carroCompra);

    }
}
