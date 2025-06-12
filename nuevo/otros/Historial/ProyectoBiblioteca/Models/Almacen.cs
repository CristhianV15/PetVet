using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Almacen
    {
        public int idalmacen { get; set; }
        public string almacen { get; set; }
        public bool estado { get; set; }

    }

    public class Codllanta
    {
        public int idscraps { get; set; }
        public string codllanta { get; set; }
        public bool estado { get; set; }

    }
}