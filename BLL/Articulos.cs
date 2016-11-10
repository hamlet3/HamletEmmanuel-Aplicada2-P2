using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Articulos : ClaseMaestra
    {
        public int ArticuloId { get; set; }
        public string Descripcion { get; set; }
        public int Existencia { get; set; }
        public int Precio { get; set; }

        public Articulos() { }

        public override bool Buscar(int IdBuscado)
        {
            throw new NotImplementedException();
        }

        public override bool Editar()
        {
            throw new NotImplementedException();
        }

        public override bool Eliminar()
        {
            throw new NotImplementedException();
        }

        public override bool Insertar()
        {
            throw new NotImplementedException();
        }

        public override DataTable Listado(string Campos, string Condicion, string Orden)
        {
            ConexionDb Conexion = new ConexionDb();
            string Ordenar = "";
            if (Orden.Equals(""))
                Ordenar = " Oreder by " + Orden;

            return Conexion.ObtenerDatos(string.Format("Select "+Campos+" From Articulos where "+Condicion + Ordenar));
            
        }
    }
}
