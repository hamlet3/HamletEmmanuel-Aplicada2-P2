using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace HamletEmmanuel_Aplicada2_P2
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LLenarDropDownList();
            }
        }


        public void LLenarDropDownList()
        {
            DataTable dt = new DataTable();
            Articulos articulo = new Articulos();
            dt = articulo.Listado("Descripcion, ArticuloId ","1=1","");

            foreach (DataRow row in dt.Rows)
            {
                ArticuloDropDownList.Items.Insert((int)row["ArticuloId"], row["Descripcion"].ToString());
            }
        }
        
        public void Limpiar()
        {
            BuscarIdTextBox.Text = "";

        }

        public List<VentasDetalle> ObtenerNuevaLista()
        {
            List<VentasDetalle> lista = new List<VentasDetalle>();
            VentasDetalle detalle = new VentasDetalle();
            lista.Add(detalle);

            return lista;
        }

        public List<VentasDetalle> ObtenerLista()
        {
            if (Session["Detalle"] == null)
            {
                return ObtenerNuevaLista();
            }
            else
            {
                return (List<VentasDetalle>)Session["Detalle"];
            }
        }

        public List<VentasDetalle> GuardatLista(VentasDetalle detalle)
        {
            if (Session["Detalle"] == null)
            {
                List<VentasDetalle> detalle2 = this.ObtenerNuevaLista();
                detalle2.Add(detalle);
                Session["Detalle"] = detalle2;
            }
            else
            {
                List<VentasDetalle> detalle2 = (List<VentasDetalle>)Session["Detalle"];
                detalle2.Add(detalle);
                Session["Detalle"] = detalle2;
            }

            return (List<VentasDetalle>)Session["Detalle"];
        }

        protected void NuevoButton_Click(object sender, EventArgs e)
        {

        }
    }
}