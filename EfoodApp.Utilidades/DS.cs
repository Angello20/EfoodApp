using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfoodApp.Utilidades
{
    public static class DS
    {
        public const string Exitosa = "Exitosa";
        public const string Error = "Error";
        public const string ImagenRuta = "/imagenes/producto/";
        public const string ssCarroCompras = "Sesion carro Compras";

        public const string Role_Admin = "Admin";
        public const string Role_Seguridad = "Seguridad";
        public const string Role_Mantenimiento = "Mantenimiento";
        public const string Role_Consulta = "Consulta";
        public const string Role_Cliente = "Cliente";

        public static readonly List<string> PreguntasSeguridad = new List<string>
        {
            "¿Cuál es el nombre de tu primera mascota?",
            "¿Cuál es tu comida favorita?",
            "¿Cuál es el nombre de la calle donde creciste?",
        };


    }
}
