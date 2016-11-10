using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    public class Ventas : ClaseMaestra
    {
        public int VentaId { get; set; }
        public string Fecha { get; set; }
        public int Monto { get; set; }

        public List<VentasDetalle> DetalleLista = new List<VentasDetalle>();

        public Ventas() { }

        public override bool Insertar()
        {
            ConexionDb conexion = new ConexionDb();
            int retorno;
            object identity;
            try
            {
                identity = conexion.ObtenerValor(String.Format("Insert into Ventas(Fecha, Monto) Values('{0}', {1}) Select @@identity", this.Fecha, this.Monto));
                int.TryParse(identity.ToString(), out retorno);

                foreach (VentasDetalle ventaDetalle in DetalleLista)
                {
                    conexion.Ejecutar(String.Format("Insert into VentasDetalle(VentaId, ArticuloId, Cantidad, Precio) Values({0}, {1}, {2}, {3})", retorno,ventaDetalle.ArticuloId , ventaDetalle.Cantidad,ventaDetalle.Precio));
                }
            }
            catch (Exception ex) { throw ex; }
            return retorno > 0;
        }

        public override bool Editar()
        {
            bool retorno = false;
            ConexionDb conexion = new ConexionDb();
            try
            {
                retorno = conexion.Ejecutar(string.Format("Update set Fecha='{0}', Monto={1} where VentaId={2} ",this.Fecha, this.Monto, this.VentaId));

                conexion.Ejecutar(String.Format("Delete from VentasDetalle where VentaId =", this.VentaId));

                foreach (VentasDetalle ventaDetalle in DetalleLista)
                {
                    conexion.Ejecutar(String.Format("Insert into VentasDetalle (VentaId, ArticuloId, Cantidad, Precio) Values({0}, {1}, {2}, {3})", this.VentaId, ventaDetalle.ArticuloId, ventaDetalle.Cantidad, ventaDetalle.Precio));
                }
            }catch(Exception ex) { throw ex; }
            return retorno;
        }

        public override bool Eliminar()
        {
            bool retorno = false;
            ConexionDb conexion = new ConexionDb();
            try
            {
                retorno = conexion.Ejecutar(string.Format("Delete from VentasDetalle where VentaId = "+ this.VentaId+";"+"Delete from Ventas where VentaId=",this.VentaId));

            }
            catch (Exception ex) { throw ex; }
            return retorno;
        }
        public override bool Buscar(int IdBuscado)
        {
            ConexionDb conexion = new ConexionDb();
            DataTable dt = new DataTable();
            DataTable DetalleDt = new DataTable();

            dt = conexion.ObtenerDatos("Select * from Ventas where VentaId=" + IdBuscado);
            if (dt.Rows.Count > 0)
            {
                this.VentaId = (int)dt.Rows[0]["VentaId"];
                this.Fecha = dt.Rows[0]["Fecha"].ToString();
                this.Monto = (int)dt.Rows[0]["Monto"];
                

                DetalleDt = conexion.ObtenerDatos(String.Format("Select * from VentasDetalle  where VentaId=" + IdBuscado));
            }
            return dt.Rows.Count > 0;
        }
        
        public override DataTable Listado(string Campos, string Condicion, string Orden)
        {
            ConexionDb Conexion = new ConexionDb();
            string Ordenar = "";
            if (Orden.Equals(""))
                Ordenar = " Order by " + Orden;

            return Conexion.ObtenerDatos(string.Format("Select " + Campos + " From Ventas where " + Condicion + Orden));

        }
    }
}
