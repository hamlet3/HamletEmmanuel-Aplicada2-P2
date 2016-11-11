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
        public List<Articulos> CantidadArticulo = new List<Articulos>();

        public void AgregarArticulo(int articuloId, int  cantidad,int precio)
        {
            DetalleLista.Add(new VentasDetalle(articuloId, cantidad, precio));
        }

        public void ObtenerArticulo(string descripcion,int articuloId, int cantidad, int precio)
        {
            DetalleLista.Add(new VentasDetalle(descripcion, articuloId, cantidad, precio));
        }

        public void AgregarCantidadArticulo(int existencia, int articuloId) {
            CantidadArticulo.Add(new Articulos(existencia,articuloId));
        }

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

                    conexion.Ejecutar(String.Format("Update Articulos set Existencia-={0} where ArticuloId={1}", ventaDetalle.Cantidad, ventaDetalle.ArticuloId));
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
                retorno = conexion.Ejecutar(string.Format("Update Ventas set Fecha='{0}', Monto={1} where VentaId={2} ",this.Fecha, this.Monto, this.VentaId));

                conexion.Ejecutar(String.Format("Delete from VentasDetalle where VentaId ={0}", this.VentaId));

                foreach (VentasDetalle ventaDetalle in DetalleLista)
                {
                    conexion.Ejecutar(String.Format("Insert into VentasDetalle (VentaId, ArticuloId, Cantidad, Precio) Values({0}, {1}, {2}, {3})", this.VentaId, ventaDetalle.ArticuloId, ventaDetalle.Cantidad, ventaDetalle.Precio));
                }
                foreach (Articulos articulo in CantidadArticulo)
                {
                    conexion.Ejecutar(String.Format("Update Articulos set Existencia={0} where ArticuloId={1}", articulo.Existencia, articulo.ArticuloId));
                }
            }
            catch(Exception ex) { throw ex; }
            return retorno;
        }

        public override bool Eliminar()
        {
            bool retorno = false;
            ConexionDb conexion = new ConexionDb();
            try
            {
                retorno = conexion.Ejecutar(string.Format("Delete from VentasDetalle where VentaId = "+ this.VentaId+";"+"Delete from Ventas where VentaId={0}",this.VentaId));

                foreach (VentasDetalle ventaDetalle in DetalleLista)
                {
                    conexion.Ejecutar(String.Format("Update Articulos set Existencia+={0} where ArticuloId={1}", ventaDetalle.Cantidad, ventaDetalle.ArticuloId));
                }

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
                

                DetalleDt = conexion.ObtenerDatos(String.Format("Select Descripcion, A.ArticuloId, Cantidad, A.Precio from VentasDetalle as VD inner join Articulos as A on Vd.ArticuloId=A.ArticuloId  where VentaId=" + IdBuscado));
                foreach (DataRow row in DetalleDt.Rows)
                {
                    ObtenerArticulo(row["Descripcion"].ToString(),(int)row["ArticuloId"], (int)row["Cantidad"],(int)row["Precio"]);
                }
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
