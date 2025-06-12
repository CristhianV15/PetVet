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
    public class LlantaLogica
    {

        private static LlantaLogica instancia = null;

        public LlantaLogica()
        {

        }

        public static LlantaLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LlantaLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Llantas objeto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLlanta", oConexion);
                    cmd.Parameters.AddWithValue("id", objeto.oVehiculo.id);
                    cmd.Parameters.AddWithValue("iddet", objeto.oVehiculodet.iddet);
                    cmd.Parameters.AddWithValue("codllanta", objeto.codllanta);
                    cmd.Parameters.AddWithValue("idmarca", objeto.oMarca.idmarca);
                    cmd.Parameters.AddWithValue("idmodelo", objeto.oModelo.idmodelo);
                    cmd.Parameters.AddWithValue("idmedida", objeto.oMedida.idmedida);
                    cmd.Parameters.AddWithValue("remanentereencauche", objeto.remanentereencauche);
                    cmd.Parameters.AddWithValue("remanente", objeto.remanente);
                    cmd.Parameters.AddWithValue("posicion", objeto.posicion);
                    cmd.Parameters.AddWithValue("kminstalacion", objeto.kminstalacion);
                    cmd.Parameters.AddWithValue("nroreencauche", objeto.nroreencauche);
                    cmd.Parameters.AddWithValue("idalmacen", objeto.oAlmacen.idalmacen);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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


        public bool Modificar(Llantas objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarLlanta", oConexion);
                    cmd.Parameters.AddWithValue("idllanta", objeto.idllanta);
                    cmd.Parameters.AddWithValue("id", objeto.oVehiculo.id);
                    cmd.Parameters.AddWithValue("iddet", objeto.oVehiculodet.iddet);
                    cmd.Parameters.AddWithValue("codllanta", objeto.codllanta);
                    cmd.Parameters.AddWithValue("idmarca", objeto.oMarca.idmarca);
                    cmd.Parameters.AddWithValue("idmodelo", objeto.oModelo.idmodelo);
                    cmd.Parameters.AddWithValue("idmedida", objeto.oMedida.idmedida);
                    cmd.Parameters.AddWithValue("remanentereencauche", objeto.remanentereencauche);
                    cmd.Parameters.AddWithValue("remanente", objeto.remanente);
                    cmd.Parameters.AddWithValue("posicion", objeto.posicion);
                    cmd.Parameters.AddWithValue("kminstalacion", objeto.kminstalacion);
                    cmd.Parameters.AddWithValue("nroreencauche", objeto.nroreencauche);
                    cmd.Parameters.AddWithValue("idalmacen", objeto.oAlmacen.idalmacen);
                    cmd.Parameters.AddWithValue("estado", objeto.estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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


        public List<Llantas> Listar()
        {

            List<Llantas> rptListaLlanta = new List<Llantas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanenteoriginal,v.remanente,v.posicion,v.kminstalacion,v.nroreencauche,v.idalmacen,g.almacen,v.estado ");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                sb.AppendLine("where v.idalmacen <> 2");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaLlanta.Add(new Llantas()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),

                            oVehiculo = new Vehiculo() { 
                                id = Convert.ToInt32(dr["id"].ToString()), 
                                tipo = dr["tipo"].ToString() 
                            },

                            oVehiculodet = new Vehiculodet() { 
                                iddet = Convert.ToInt32(dr["iddet"].ToString()), 
                                unidad = dr["unidad"].ToString() 
                            },

                            codllanta = dr["codllanta"].ToString(),

                            oMarca = new Marca() { 
                                idmarca = Convert.ToInt32(dr["idmarca"].ToString()), 
                                marca = dr["marca"].ToString() 
                            },

                            oModelo = new Modelo() { 
                                idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), 
                                modelo = dr["modelo"].ToString() 
                            },

                            oMedida = new Medida() { 
                                idmedida = Convert.ToInt32(dr["idmedida"].ToString()), 
                                medida = dr["medida"].ToString() 
                            },
                            remanenteoriginal = Convert.ToInt32(dr["remanenteoriginal"].ToString()),
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            kminstalacion = Convert.ToInt32(dr["kminstalacion"].ToString()),
                            nroreencauche = Convert.ToInt32(dr["nroreencauche"].ToString()),

                            oAlmacen = new Almacen() { 
                                idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()), 
                                almacen = dr["almacen"].ToString() 
                            },

                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaLlanta;

                }
                catch (Exception ex)
                {
                    rptListaLlanta = null;
                    return rptListaLlanta;
                }
            }
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from LLANTA where idllanta = @idllanta", oConexion);
                    cmd.Parameters.AddWithValue("@idllanta", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

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