﻿using EfoodApp.AccesoDatos.Data;
using EfoodApp.AccesoDatos.Repositorio.IRepositorio;
using EfoodApp.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.AccesoDatos.Repositorio
{
    public class CarroCompraRepositorio : Repositorio<CarroCompra>, ICarroCompraRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CarroCompraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(CarroCompra carroCompra)
        {
            _db.Update(carroCompra);
        }
    }


}
