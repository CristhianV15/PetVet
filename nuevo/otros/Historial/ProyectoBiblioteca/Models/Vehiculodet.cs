using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Vehiculodet
    {
        public int iddet { get; set; }
        public Vehiculo oVehiculo { get; set; }
        public string unidad { get; set; }
        public string marca { get; set; }
        public string placa { get; set; }
        public int eje { get; set; }
        public bool estado { get; set; }

    }
}