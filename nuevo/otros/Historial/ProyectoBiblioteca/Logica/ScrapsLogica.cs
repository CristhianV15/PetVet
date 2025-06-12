using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoBiblioteca.Logica
{
    public class ScrapsLogica
    {

        private static ScrapsLogica instancia = null;

        public ScrapsLogica()
        {

        }

        public static ScrapsLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ScrapsLogica();
                }

                return instancia;
            }
        }

        public List<Scraps> Listar()
        {

            List<Scraps> rptListaScraps = new List<Scraps>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanente,v.estado ");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("WHERE v.idalmacen <> 2");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaScraps.Add(new Scraps()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),
                            id = Convert.ToInt32(dr["id"].ToString()),
                            oVehiculo = new Vehiculo() { id = Convert.ToInt32(dr["id"].ToString()), tipo = dr["tipo"].ToString() },
                            iddet = Convert.ToInt32(dr["iddet"].ToString()),
                            oVehiculodet = new Vehiculodet() { iddet = Convert.ToInt32(dr["iddet"].ToString()), unidad = dr["unidad"].ToString() },
                            codllanta = dr["codllanta"].ToString(),
                            idmarca = Convert.ToInt32(dr["idmarca"].ToString()),
                            oMarca = new Marca() { idmarca = Convert.ToInt32(dr["idmarca"].ToString()), marca = dr["marca"].ToString() },
                            idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()),
                            oModelo = new Modelo() { idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), modelo = dr["modelo"].ToString() },
                            idmedida = Convert.ToInt32(dr["idmedida"].ToString()),
                            oMedida = new Medida() { idmedida = Convert.ToInt32(dr["idmedida"].ToString()), medida = dr["medida"].ToString() },
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaScraps;

                }
                catch (Exception ex)
                {
                    rptListaScraps = null;
                    return rptListaScraps;
                }
            }
        }


        public int Registrar(Scraps objeto)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarScraps", oConexion);
                    cmd.Parameters.AddWithValue("RutaPortada", objeto.RutaPortada);
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.NombrePortada);
                    cmd.Parameters.AddWithValue("id", objeto.oVehiculo.id);
                    cmd.Parameters.AddWithValue("iddet", objeto.oVehiculodet.iddet);
                    cmd.Parameters.AddWithValue("codllanta", objeto.codllanta);
                    cmd.Parameters.AddWithValue("idmarca", objeto.oMarca.idmarca);
                    cmd.Parameters.AddWithValue("idmodelo", objeto.oModelo.idmodelo);
                    cmd.Parameters.AddWithValue("idmedida", objeto.oMedida.idmedida);
                    cmd.Parameters.AddWithValue("remanente", objeto.remanente);
                    cmd.Parameters.AddWithValue("observaciones", objeto.observaciones);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public bool ActualizarRutaImagen(Scraps objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("idscraps", objeto.idscraps);
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.NombrePortada);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

    }
}