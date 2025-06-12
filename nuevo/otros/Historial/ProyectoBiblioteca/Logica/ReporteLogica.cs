using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoBiblioteca.Logica
{
    public class ReporteR
    {

        private static ReporteR instancia = null;

        public ReporteR()
        {

        }

        public static ReporteR Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ReporteR();
                }

                return instancia;
            }
        }

        public List<ReporteRemanente> RetornarRemanente()
        {
            List<ReporteRemanente> objLista = new List<ReporteRemanente>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                string query = "sp_ReporteRemanentes";

                SqlCommand cmd = new SqlCommand(query, oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteRemanente()
                        {
                            remanentes = dr["remanentes"].ToString(),
                            cantidad = int.Parse(dr["cantidad"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }

        public List<ReporteporMarcas> RetornarMarca()
        {
            List<ReporteporMarcas> objLista = new List<ReporteporMarcas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                string query = "sp_ReporteMarcas";

                SqlCommand cmd = new SqlCommand(query, oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteporMarcas()
                        {
                            marca = dr["marca"].ToString(),
                            cantidad = int.Parse(dr["cantidad"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }

        public List<ReporteporEstado> RetornarEstado()
        {
            List<ReporteporEstado> objLista = new List<ReporteporEstado>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                string query = "sp_ReporteEstado";

                SqlCommand cmd = new SqlCommand(query, oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteporEstado()
                        {
                            estadooperacion = dr["estadooperacion"].ToString(),
                            cantidad = int.Parse(dr["cantidad"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }

        public List<ReporteMarcas> ListarReporteMarcas()
        {

            List<ReporteMarcas> rptReporteMarca = new List<ReporteMarcas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.marca, v.modelo, SUM(v.cantidad)[cantidad], CONVERT(VARCHAR(100),");
                sb.AppendLine("CONVERT(DECIMAL(10, 1), (SUM(v.cantidad) * 100 / (Select count(codllanta) FROM LLANTA)))) +' ' + '%'[porcentaje]");
                sb.AppendLine("FROM(SELECT m.marca, n.modelo, COUNT(codllanta) cantidad FROM[DB_NEUMATICOS].[dbo].[LLANTA] l");
                sb.AppendLine("LEFT JOIN[dbo].[MARCA] m ON m.idmarca = l.idmarca");
                sb.AppendLine("LEFT JOIN[dbo].[MODELO] n ON n.idmodelo = l.idmodelo");
                sb.AppendLine("WHERE l.idalmacen = 1 AND l.estado = 1");
                sb.AppendLine("GROUP BY m.marca, n.modelo) as v");
                sb.AppendLine("GROUP BY v.marca, v.modelo");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptReporteMarca.Add(new ReporteMarcas()
                        {
                            marca = dr["marca"].ToString(),
                            modelo = dr["modelo"].ToString(),
                            cantidad = Convert.ToInt32(dr["cantidad"].ToString()),
                            porcentaje = dr["porcentaje"].ToString()
                        });
                    }
                    dr.Close();

                    return rptReporteMarca;

                }
                catch (Exception ex)
                {
                    rptReporteMarca = null;
                    return rptReporteMarca;
                }
            }
        }

    }

    
}