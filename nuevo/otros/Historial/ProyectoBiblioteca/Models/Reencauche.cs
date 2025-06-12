using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Reencauche
    {

        public int idremanente { get; set; }
        public int idllanta { get; set; }
        public int id { get; set; }
        public int iddet { get; set; }
        public string codllanta { get; set; }
        public int posicion { get; set; }
        public int idmarca { get; set; }
        public int idmodelo { get; set; }
        public int idmedida { get; set; }
        public int idalmacen { get; set; }
        public int anoinstalacion { get; set; }
        public int mesinstalacion { get; set; }
        public int kminstalacion { get; set; }
        public int nroreencauche { get; set; }
        public string fechainspeccion { get; set; }
        public int kminspeccion { get; set; }
        public int kmrecorrido { get; set; }
        public int remanenteactual { get; set; }
        public int remanentereencauche { get; set; }
        public int remanentedesgastado { get; set; }
        public string estadooperacion { get; set; }
        public string observaciones { get; set; }
        public bool estado { get; set; }
        public Llantas oLlanta { get; set; }
        public Vehiculo oVehiculo { get; set; }
        public Vehiculodet oVehiculodet { get; set; }
        public Marca oMarca { get; set; }
        public Modelo oModelo { get; set; }
        public Medida oMedida { get; set; }
        public Almacen oAlmacen { get; set; }

    }

    public class ReencaucheMod
    {
        public int idremanente { get; set; }
        public string codllanta { get; set; }
        public string fechainspeccion { get; set; }
        public int kminspeccion { get; set; }
        public int kmrecorrido { get; set; }
        public int remanenteactual { get; set; }
        public string estadooperacion { get; set; }
        public string observaciones { get; set; }

    }
}