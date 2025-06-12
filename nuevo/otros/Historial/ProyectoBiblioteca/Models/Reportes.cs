using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class ReporteRemanente
    {
        public string remanentes { get; set; }
        public int cantidad { get; set; }

    }

    public class ReporteporMarcas
    {
        public string marca { get; set; }
        public int cantidad { get; set; }

    }

    public class ReporteporEstado
    {
        public string estadooperacion { get; set; }
        public int cantidad { get; set; }

    }

    public class ReporteMarcas
    {
        public string marca { get; set; }
        public string modelo { get; set; }
        public int cantidad { get; set; }
        public string porcentaje { get; set; }

    }

    public class Notificaciones
    {
        public string codllanta { get; set; }
        public int remanente { get; set; }
        public bool estado { get; set; }

    }
}