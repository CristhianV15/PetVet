using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProyectoBiblioteca.Logica
{
    public class ReencaucheLogica
    {

        private static ReencaucheLogica instancia = null;

        public ReencaucheLogica()
        {

        }

        public static ReencaucheLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ReencaucheLogica();
                }

                return instancia;
            }
        }

        public bool Modificar(ReencaucheMod Objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarReencauche", oConexion);
                    cmd.Parameters.AddWithValue("idremanente", Objeto.idremanente);
                    cmd.Parameters.AddWithValue("codllanta", Objeto.codllanta);
                    cmd.Parameters.AddWithValue("fechainspeccion", Objeto.fechainspeccion);
                    cmd.Parameters.AddWithValue("kminspeccion", Objeto.kminspeccion);
                    cmd.Parameters.AddWithValue("remanenteactual", Objeto.remanenteactual);
                    cmd.Parameters.AddWithValue("estadooperacion", Objeto.estadooperacion);
                    cmd.Parameters.AddWithValue("observaciones", Objeto.observaciones);
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


        public List<Reencauche> Listar()
        {

            List<Reencauche> rptListaReencauche = new List<Reencauche>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idremanente,h.id,b.tipo,h.iddet,c.unidad,v.codllanta,h.idmarca,d.marca,h.idmodelo,e.modelo,h.idmedida,f.medida,");
                sb.AppendLine("convert(int,'20' + left(v.codllanta,2))[anoinstalacion],convert(int,substring(v.codllanta,3,2))[mesinstalacion],h.posicion,h.nroreencauche,v.fechainspeccion,");
                sb.AppendLine("h.kminstalacion,v.kminspeccion,v.kminspeccion - h.kminstalacion[kmrecorrido],h.remanentereencauche,v.remanenteactual,h.remanentereencauche - v.remanenteactual[remanentedesgastado],");
                sb.AppendLine("v.estadooperacion,v.observaciones,h.idalmacen,g.almacen,v.estado");
                sb.AppendLine("from REMANENTE v");
                sb.AppendLine("left join LLANTA h ON h.codllanta = v.codllanta");
                sb.AppendLine("left join VEHICULO b on b.id = h.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = h.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = h.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = h.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = h.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = h.idalmacen");
                sb.AppendLine("where v.estado = 1");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaReencauche.Add(new Reencauche()
                        {
                            idremanente = Convert.ToInt32(dr["idremanente"].ToString()),

                            oVehiculo = new Vehiculo()
                            {
                                id = Convert.ToInt32(dr["id"].ToString()),

                                tipo = dr["tipo"].ToString()
                            },

                            oVehiculodet = new Vehiculodet()
                            {
                                iddet = Convert.ToInt32(dr["iddet"].ToString()),
                                unidad = dr["unidad"].ToString()
                            },

                            codllanta = dr["codllanta"].ToString(),

                            oMarca = new Marca()
                            {
                                idmarca = Convert.ToInt32(dr["idmarca"].ToString()),
                                marca = dr["marca"].ToString()
                            },

                            oModelo = new Modelo()
                            {
                                idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()),
                                modelo = dr["modelo"].ToString()
                            },

                            oMedida = new Medida()
                            {
                                idmedida = Convert.ToInt32(dr["idmedida"].ToString()),
                                medida = dr["medida"].ToString()
                            },
                            anoinstalacion = Convert.ToInt32(dr["anoinstalacion"].ToString()),
                            mesinstalacion = Convert.ToInt32(dr["mesinstalacion"].ToString()),
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            nroreencauche = Convert.ToInt32(dr["nroreencauche"].ToString()),
                            fechainspeccion = dr["fechainspeccion"].ToString(),
                            kminstalacion = Convert.ToInt32(dr["kminstalacion"].ToString()),
                            kminspeccion = Convert.ToInt32(dr["kminspeccion"].ToString()),
                            kmrecorrido = Convert.ToInt32(dr["kmrecorrido"].ToString()),
                            remanentereencauche = Convert.ToInt32(dr["remanentereencauche"].ToString()),
                            remanenteactual = Convert.ToInt32(dr["remanenteactual"].ToString()),
                            remanentedesgastado = Convert.ToInt32(dr["remanentedesgastado"].ToString()),
                            estadooperacion = dr["estadooperacion"].ToString(),
                            observaciones = dr["observaciones"].ToString(),

                            oAlmacen = new Almacen()
                            {
                                idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()),
                                almacen = dr["almacen"].ToString()
                            },

                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaReencauche;

                }
                catch (Exception ex)
                {
                    rptListaReencauche = null;
                    return rptListaReencauche;
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
                    SqlCommand cmd = new SqlCommand("delete from REMANENTE where idremanente = @idremanente", oConexion);
                    cmd.Parameters.AddWithValue("@idremanente", id);
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

        public DataTable ExpInspecciones(string fechainicio, string fechafin)
        {

            DataTable dt = new DataTable();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT v.idremanente,h.id,b.tipo,h.iddet,c.unidad,v.codllanta,h.idmarca,d.marca,h.idmodelo,e.modelo,h.idmedida,f.medida,");
                    sb.AppendLine("convert(int,'20' + left(v.codllanta,2))[anoinstalacion],convert(int,substring(v.codllanta,3,2))[mesinstalacion],h.posicion,h.nroreencauche,v.fechainspeccion,");
                    sb.AppendLine("h.kminstalacion,v.kminspeccion,v.kminspeccion - h.kminstalacion[kmrecorrido],h.remanentereencauche,v.remanenteactual,h.remanentereencauche - v.remanenteactual[remanentedesgastado],");
                    sb.AppendLine("v.estadooperacion,v.observaciones,h.idalmacen,g.almacen,v.estado");
                    sb.AppendLine("from REMANENTE v");
                    sb.AppendLine("left join LLANTA h ON h.codllanta = v.codllanta");
                    sb.AppendLine("left join VEHICULO b on b.id = h.id");
                    sb.AppendLine("left join VEHICULODET c on c.iddet = h.iddet");
                    sb.AppendLine("left join MARCA d ON d.idmarca = h.idmarca");
                    sb.AppendLine("left join MODELO e ON e.idmodelo = h.idmodelo");
                    sb.AppendLine("left join MEDIDA f ON f.idmedida = h.idmedida");
                    sb.AppendLine("left join ALMACEN g ON g.idalmacen = h.idalmacen");
                    sb.AppendLine("where convert(date, v.fechainspeccion,103) between @fechainicio and @fechafin");
                    sb.AppendLine("and v.estado = 1");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("@fechafin", fechafin);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                }

                catch (Exception ex)
                {
                    dt = new DataTable();
                }

            }
            return dt;

        }
    }
}