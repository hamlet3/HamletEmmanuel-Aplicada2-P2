﻿using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class VentasDetalle : ClaseMaestra
    {

        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ArticuloId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }

        public VentasDetalle() { }

        public VentasDetalle(int articuloId,int cantidad, int precio)
        {
            this.ArticuloId = articuloId;
            this.Cantidad = cantidad;
            this.Precio = precio;
        }

        public VentasDetalle(string descripcion,int articuloId, int cantidad, int precio)
        {
            this.Descripcion = descripcion;
            this.ArticuloId = articuloId;
            this.Cantidad = cantidad;
            this.Precio = precio;
        }

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

            return Conexion.ObtenerDatos(string.Format("Select " + Campos + " From VentasDetalle where " + Condicion + Ordenar));

        }
    }
}
