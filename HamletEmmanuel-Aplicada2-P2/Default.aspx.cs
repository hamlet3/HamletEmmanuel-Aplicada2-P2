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
            dt = articulo.Listado("Descripcion, ArticuloId ", "1=1", "");

            foreach (DataRow row in dt.Rows)
            {
                ArticuloDropDownList.Items.Insert((int)row["ArticuloId"], row["Descripcion"].ToString());
            }
        }

        public void Limpiar(bool aux)
        {
            if (aux)
            {
                BuscarIdTextBox.Text = "";
                CantidadTextBox.Text = "";
                ArticuloDropDownList.SelectedIndex = 0;
                Session.Clear();
                DetalleGridView.DataSource = ObtenerNuevaLista();
                DetalleGridView.DataBind();
                MontoLabel.Text = 0.ToString();
                ControlDeBotones(3);
            }
            else
            {
                CantidadTextBox.Text = "";
                ArticuloDropDownList.SelectedIndex = 0;
                Session.Clear();
                DetalleGridView.DataSource = ObtenerNuevaLista();
                DetalleGridView.DataBind();
                MontoLabel.Text = 0.ToString();
                ControlDeBotones(3);
            }
        }

        public void ControlDeBotones(int control)
        {
            if (control==1)
            {
                GuardarButton.Enabled = true;
                EliminarButton.Enabled = false;
            }
            if(control==2)
            {
                GuardarButton.Enabled = true;
                EliminarButton.Enabled = true;
            }
            if (control == 3)
            {
                GuardarButton.Enabled = false;
                EliminarButton.Enabled = false;
            }
        }

        public void Mensaje(string mensaje)
        {
            Response.Write("<script>alert('" + mensaje + "')</script>");
        }


        public int ConvertirValor(string convertir)
        {
            int aux;
            int.TryParse(convertir, out aux);
            return aux;
        }

        public List<VentasDetalle> ObtenerNuevaLista()
        {
            List<VentasDetalle> lista = new List<VentasDetalle>();
            

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
            Limpiar(true);
        }

        protected void AgregarButton_Click(object sender, EventArgs e)
        {
            VentasDetalle ventaDetalle = new VentasDetalle();
            Articulos articulo = new Articulos();
            if (ArticuloDropDownList.SelectedIndex != 0)
            {
                ControlDeBotones(1);
                articulo.ArticuloId = ArticuloDropDownList.SelectedIndex;
                articulo.Buscar(articulo.ArticuloId);
                if (articulo.Existencia > ConvertirValor(CantidadTextBox.Text))
                {
                    Ventas venta;
                    if (Session["Venta"] == null)
                        Session["Venta"] = new Ventas();

                    venta = (Ventas)Session["Venta"];
                    venta.AgregarArticulo(articulo.ArticuloId, ConvertirValor(CantidadTextBox.Text), articulo.Precio);

                    ventaDetalle.Descripcion = articulo.Descripcion;
                    ventaDetalle.Cantidad = ConvertirValor(CantidadTextBox.Text);
                    ventaDetalle.Precio = articulo.Precio;


                    venta.AgregarCantidadArticulo(articulo.Existencia - ventaDetalle.Cantidad, articulo.ArticuloId);
                    Session["Venta"] = venta;

                    GuardatLista(ventaDetalle);
                    DetalleGridView.DataSource = ObtenerLista();
                    DetalleGridView.DataBind();

                    venta.Monto += articulo.Precio * ventaDetalle.Cantidad;
                    MontoLabel.Text = venta.Monto.ToString();

                    ArticuloDropDownList.SelectedIndex = 0;
                    CantidadTextBox.Text = "";
                }
                else
                    Mensaje("Usted a sobrepasado la cantidad maxima de este articulo");
            }
            else
            {
                Mensaje("Seleccione un articulo");
            }
        }

        protected void BorrarButton_Click(object sender, EventArgs e)
        {

        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {
            Ventas venta;
            if (Session["Venta"] == null)
                Session["Venta"] = new Ventas();
                venta=(Ventas)Session["Venta"];


            if (BuscarIdTextBox.Text == "")
            {
                venta.Monto = ConvertirValor(MontoLabel.Text);
                venta.Fecha = DateTime.Now.ToString("dd/MM/yyyy");

                if (venta.Insertar())
                {
                    Mensaje("Exito al guardar");
                    Limpiar(true);
                }
                else
                    Mensaje("Error al guardar");
            }
            else
            {
                venta.Monto = ConvertirValor(MontoLabel.Text);
                venta.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                venta.VentaId = ConvertirValor(BuscarIdTextBox.Text);
                
                if (venta.Editar())
                {
                    Mensaje("Exito al editar");
                    Limpiar(true);
                }
                else
                    Mensaje("Error al editar");
            }
        }

        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            Ventas venta = new Ventas();
            venta =(Ventas) Session["Venta"];

            if (venta.Eliminar())
            {
                Mensaje("Exito al eliminar");
                Limpiar(true);
            }
            else
                Mensaje("Error al eliminar");
        }

        protected void BuscarIdButton_Click(object sender, EventArgs e)
        {
            if (BuscarIdTextBox.Text!="")
            {
                Ventas venta = new Ventas();
                if (venta.Buscar(ConvertirValor(BuscarIdTextBox.Text))){
                    Limpiar(false);
                    Session["Venta"]= venta;
                    ControlDeBotones(2);
                    MontoLabel.Text = venta.Monto.ToString();
                    foreach (VentasDetalle detalle in venta.DetalleLista)
                    {
                        GuardatLista(detalle);
                    }

                    DetalleGridView.DataSource = ObtenerLista();
                    DetalleGridView.DataBind();
                }else
                    Mensaje("Id no encontrado");
            }
        }
    }
}