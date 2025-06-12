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
    public class CrearRegistroLogica
    {

        public static List<CrearRegisto> Listar()
        {
            List<CrearRegisto> rptListarReencauche = new List<CrearRegisto>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarReencauche", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        rptListarReencauche.Add(new CrearRegisto()
                        {
 
                            oVehiculo = new Vehiculo()
                            {
                                id = Convert.ToInt32(dr["id"].ToString()),
                                tipo = dr["tipo"].ToString(),
                            },

                            oVehiculodet = new Vehiculodet()
                            {
                                iddet = Convert.ToInt32(dr["iddet"].ToString()),
                                unidad = dr["unidad"].ToString(),
                                
                            },
                            codllanta = dr["codllanta"].ToString(),
                            oMarca = new Marca()
                            {
                                idmarca = Convert.ToInt32(dr["idmarca"].ToString()),
                                marca = dr["marca"].ToString(),

                            },
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            kminstalacion = Convert.ToInt32(dr["kminstalacion"].ToString()),
                            nroreencauche = Convert.ToInt32(dr["nroreencauche"].ToString()),
                            fechainspeccion = dr["fechainspeccion"].ToString(),
                            kminspeccion = Convert.ToInt32(dr["kminspeccion"].ToString()),
                            remanenteactual = Convert.ToInt32(dr["remanenteactual"].ToString()),
                            estadooperacion = dr["estadooperacion"].ToString(),
                            observaciones =dr["observaciones"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"])

                        });
                    }
                    dr.Close();

                    return rptListarReencauche;

                }
                catch (Exception ex)
                {
                    rptListarReencauche = null;
                    return rptListarReencauche;
                }
            }
        }

        public static bool Registrar(string xml)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarReencauche", oConexion);
                    cmd.Parameters.Add("xml", SqlDbType.Xml).Value = xml;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

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