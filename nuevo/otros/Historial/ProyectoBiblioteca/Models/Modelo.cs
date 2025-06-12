using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Modelo
    {
        public int idmodelo { get; set; }
        public string modelo { get; set; }
        public decimal precio { get; set; }
        public bool estado { get; set; }

    }
}