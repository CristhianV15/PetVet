using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Llantas
    {
        public int idllanta { get; set; }
        public string codllanta { get; set; }
        public int posicion { get; set; }
        public int remanenteoriginal { get; set; }
        public int remanentereencauche { get; set; }
        public int remanente { get; set; }
        public int kminstalacion { get; set; }
        public int nroreencauche { get; set; }
        public bool estado { get; set; }
        public int alerta { get; set; }
        public Vehiculo oVehiculo { get; set; }
        public Vehiculodet oVehiculodet { get; set; }
        public Marca oMarca { get; set; }
        public Modelo oModelo { get; set; }
        public Medida oMedida { get; set; }
        public Almacen oAlmacen { get; set; }

    }
}