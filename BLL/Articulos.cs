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

        public Articulos(int existencia, int articuloId)
        {
            this.Existencia = existencia;
            this.ArticuloId = articuloId;
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

        public override bool Buscar(int IdBuscado)
        {
            DataTable dt = new DataTable();
            ConexionDb conexion = new ConexionDb();

            dt = conexion.ObtenerDatos(String.Format("Select * from Articulos where ArticuloId=" + IdBuscado));
            if (dt.Rows.Count > 0)
            {
                this.ArticuloId = (int)dt.Rows[0]["ArticuloId"];
                this.Descripcion = dt.Rows[0]["Descripcion"].ToString();
                this.Existencia = (int)dt.Rows[0]["Existencia"];
                this.Precio = (int)dt.Rows[0]["Precio"];
            }
            return dt.Rows.Count > 0;
        }
        public override DataTable Listado(string Campos, string Condicion, string Orden)
        {
            ConexionDb Conexion = new ConexionDb();
            string Ordenar = "";
            if (Orden.Equals(""))
                Ordenar = " Order by " + Orden;

            return Conexion.ObtenerDatos(string.Format("Select "+Campos+" From Articulos where "+Condicion + Orden));
            
        }
    }
}
