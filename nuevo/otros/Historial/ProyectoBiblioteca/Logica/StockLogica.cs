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
    public class StockLogica
    {

        private static StockLogica instancia = null;

        public StockLogica()
        {

        }

        public static StockLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new StockLogica();
                }

                return instancia;
            }
        }

        
        public List<Llantas> ListarTaller()
        {

            List<Llantas> rptListaTaller = new List<Llantas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado,");
                sb.AppendLine("CASE WHEN v.remanente <= 10 THEN 0 ELSE 1 END AS alerta");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                sb.AppendLine("WHERE v.idalmacen = 1");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTaller.Add(new Llantas()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),
                            oVehiculo = new Vehiculo() { id = Convert.ToInt32(dr["id"].ToString()), tipo = dr["tipo"].ToString() },
                            oVehiculodet = new Vehiculodet() { iddet = Convert.ToInt32(dr["iddet"].ToString()), unidad = dr["unidad"].ToString() },
                            codllanta = dr["codllanta"].ToString(),
                            oMarca = new Marca() { idmarca = Convert.ToInt32(dr["idmarca"].ToString()), marca = dr["marca"].ToString() },
                            oModelo = new Modelo() { idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), modelo = dr["modelo"].ToString() },
                            oMedida = new Medida() { idmedida = Convert.ToInt32(dr["idmedida"].ToString()), medida = dr["medida"].ToString() },
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            oAlmacen = new Almacen() { idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()), almacen = dr["almacen"].ToString() },
                            estado = Convert.ToBoolean(dr["estado"].ToString()),
                            alerta = Convert.ToInt32(dr["alerta"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaTaller;

                }
                catch (Exception ex)
                {
                    rptListaTaller = null;
                    return rptListaTaller;
                }
            }
        }

        public List<Llantas> ListarScraps()
        {

            List<Llantas> rptListaScraps = new List<Llantas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado ");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                sb.AppendLine("WHERE v.idalmacen = 2");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaScraps.Add(new Llantas()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),
                            oVehiculo = new Vehiculo() { id = Convert.ToInt32(dr["id"].ToString()), tipo = dr["tipo"].ToString() },
                            oVehiculodet = new Vehiculodet() { iddet = Convert.ToInt32(dr["iddet"].ToString()), unidad = dr["unidad"].ToString() },
                            codllanta = dr["codllanta"].ToString(),
                            oMarca = new Marca() { idmarca = Convert.ToInt32(dr["idmarca"].ToString()), marca = dr["marca"].ToString() },
                            oModelo = new Modelo() { idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), modelo = dr["modelo"].ToString() },
                            oMedida = new Medida() { idmedida = Convert.ToInt32(dr["idmedida"].ToString()), medida = dr["medida"].ToString() },
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            oAlmacen = new Almacen() { idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()), almacen = dr["almacen"].ToString() },
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

        public List<Llantas> ListarAuxilio()
        {

            List<Llantas> rptListaAuxilio = new List<Llantas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado ");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                sb.AppendLine("WHERE v.idalmacen = 5");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaAuxilio.Add(new Llantas()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),
                            oVehiculo = new Vehiculo() { id = Convert.ToInt32(dr["id"].ToString()), tipo = dr["tipo"].ToString() },
                            oVehiculodet = new Vehiculodet() { iddet = Convert.ToInt32(dr["iddet"].ToString()), unidad = dr["unidad"].ToString() },
                            codllanta = dr["codllanta"].ToString(),
                            oMarca = new Marca() { idmarca = Convert.ToInt32(dr["idmarca"].ToString()), marca = dr["marca"].ToString() },
                            oModelo = new Modelo() { idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), modelo = dr["modelo"].ToString() },
                            oMedida = new Medida() { idmedida = Convert.ToInt32(dr["idmedida"].ToString()), medida = dr["medida"].ToString() },
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            oAlmacen = new Almacen() { idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()), almacen = dr["almacen"].ToString() },
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaAuxilio;

                }
                catch (Exception ex)
                {
                    rptListaAuxilio = null;
                    return rptListaAuxilio;
                }
            }
        }

        public List<Llantas> ListarEstado()
        {

            List<Llantas> rptListaTaller = new List<Llantas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado,");
                sb.AppendLine("CASE WHEN v.remanente <= 10 THEN 0 ELSE 1 END AS alerta");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                sb.AppendLine("WHERE v.idalmacen = 1 and v.estado = 1 and v.remanente <= 10");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTaller.Add(new Llantas()
                        {
                            idllanta = Convert.ToInt32(dr["idllanta"].ToString()),
                            oVehiculo = new Vehiculo() { id = Convert.ToInt32(dr["id"].ToString()), tipo = dr["tipo"].ToString() },
                            oVehiculodet = new Vehiculodet() { iddet = Convert.ToInt32(dr["iddet"].ToString()), unidad = dr["unidad"].ToString() },
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            codllanta = dr["codllanta"].ToString(),
                            oMarca = new Marca() { idmarca = Convert.ToInt32(dr["idmarca"].ToString()), marca = dr["marca"].ToString() },
                            oModelo = new Modelo() { idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()), modelo = dr["modelo"].ToString() },
                            oMedida = new Medida() { idmedida = Convert.ToInt32(dr["idmedida"].ToString()), medida = dr["medida"].ToString() },
                            posicion = Convert.ToInt32(dr["posicion"].ToString()),
                            oAlmacen = new Almacen() { idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()), almacen = dr["almacen"].ToString() },
                            estado = Convert.ToBoolean(dr["estado"].ToString()),
                            alerta = Convert.ToInt32(dr["alerta"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaTaller;

                }
                catch (Exception ex)
                {
                    rptListaTaller = null;
                    return rptListaTaller;
                }
            }
        }

        public List<Codllanta> Listar()
        {

            List<Codllanta> rptListacodllanta = new List<Codllanta>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT [idscraps],[codllanta],[estado] ");
                sb.AppendLine("from [DB_NEUMATICOS].[dbo].[SCRAPS]");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListacodllanta.Add(new Codllanta()
                        {
                            idscraps = Convert.ToInt32(dr["idscraps"].ToString()),
                            codllanta = dr["codllanta"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListacodllanta;

                }
                catch (Exception ex)
                {
                    rptListacodllanta = null;
                    return rptListacodllanta;
                }
            }
        }

        public DataTable Expllantastaller()
        {

            DataTable dt = new DataTable();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                    sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado,");
                    sb.AppendLine("CASE WHEN v.remanente <= 10 THEN 'Realizar Cambio' ELSE 'Buen Estado' END AS alerta");
                    sb.AppendLine("from LLANTA v");
                    sb.AppendLine("left join VEHICULO b on b.id = v.id");
                    sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                    sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                    sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                    sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                    sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                    sb.AppendLine("WHERE v.idalmacen = 1");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
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

        public DataTable Expllantasauxilio()
        {

            DataTable dt = new DataTable();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                    sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado ");
                    sb.AppendLine("from LLANTA v");
                    sb.AppendLine("left join VEHICULO b on b.id = v.id");
                    sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                    sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                    sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                    sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                    sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                    sb.AppendLine("WHERE v.idalmacen = 5");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
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

        public DataTable Expllantasscraps()
        {

            DataTable dt = new DataTable();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                    sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado ");
                    sb.AppendLine("from LLANTA v");
                    sb.AppendLine("left join VEHICULO b on b.id = v.id");
                    sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                    sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                    sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                    sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                    sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                    sb.AppendLine("WHERE v.idalmacen = 2");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
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

        public DataTable ExpEstado()
        {

            DataTable dt = new DataTable();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT v.idllanta,v.id,b.tipo,v.iddet,c.unidad,v.codllanta,v.idmarca,d.marca,v.idmodelo,e.modelo,v.idmedida,f.medida,");
                    sb.AppendLine("v.remanente,v.posicion,v.idalmacen,g.almacen,v.estado,");
                    sb.AppendLine("CASE WHEN v.remanente <= 10 THEN 'Realizar Cambio' ELSE 'Buen Estado' END AS alerta");
                    sb.AppendLine("from LLANTA v");
                    sb.AppendLine("left join VEHICULO b on b.id = v.id");
                    sb.AppendLine("left join VEHICULODET c on c.iddet = v.iddet");
                    sb.AppendLine("left join MARCA d ON d.idmarca = v.idmarca");
                    sb.AppendLine("left join MODELO e ON e.idmodelo = v.idmodelo");
                    sb.AppendLine("left join MEDIDA f ON f.idmedida = v.idmedida");
                    sb.AppendLine("left join ALMACEN g ON g.idalmacen = v.idalmacen");
                    sb.AppendLine("WHERE v.idalmacen = 1 and v.estado = 1 and v.remanente <= 10");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
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

        public List<Notificaciones> Notificaciones()
        {

            List<Notificaciones> rptNotificaciones = new List<Notificaciones>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT v.codllanta, v.remanente,v.estado ");
                sb.AppendLine("from LLANTA v");
                sb.AppendLine("WHERE v.remanente <= 10 and v.estado = 1");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptNotificaciones.Add(new Notificaciones()
                        {
                            codllanta = dr["codllanta"].ToString(),
                            remanente = Convert.ToInt32(dr["remanente"].ToString()),
                            estado = Convert.ToBoolean(dr["estado"].ToString()),
                        });
                    }
                    dr.Close();

                    return rptNotificaciones;

                }
                catch (Exception ex)
                {
                    rptNotificaciones = null;
                    return rptNotificaciones;
                }
            }
        }

    }
}