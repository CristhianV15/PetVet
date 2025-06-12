using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Scraps
    {
        public int idscraps { get; set; }
        public int idllanta { get; set; }
        public string RutaPortada { get; set; }
        public string NombrePortada { get; set; }
        public int id { get; set; }
        public int iddet { get; set; }
        public string codllanta { get; set; }
        public int idmarca { get; set; }
        public int idmodelo { get; set; }
        public int idmedida { get; set; }
        public int remanente { get; set; }
        public string observaciones { get; set; }
        public bool estado { get; set; }
        public Vehiculo oVehiculo { get; set; }
        public Vehiculodet oVehiculodet { get; set; }
        public Marca oMarca { get; set; }
        public Modelo oModelo { get; set; }
        public Medida oMedida { get; set; }
        public Almacen oAlmacen { get; set; }
    }
}