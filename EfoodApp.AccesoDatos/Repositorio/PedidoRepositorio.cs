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
    public class PedidoRepositorio : Repositorio<Pedido>, IPedidoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PedidoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Pedido pedido)
        {
            _db.Update(pedido);
        }
    }


}
