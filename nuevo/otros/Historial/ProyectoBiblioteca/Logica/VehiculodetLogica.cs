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
    public class VehiculodetLogica
    {

        private static VehiculodetLogica instancia = null;

        public VehiculodetLogica()
        {

        }

        public static VehiculodetLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new VehiculodetLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Vehiculodet objeto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVehiculodet", oConexion);
                    cmd.Parameters.AddWithValue("id", objeto.oVehiculo.id);
                    cmd.Parameters.AddWithValue("unidad", objeto.unidad);
                    cmd.Parameters.AddWithValue("marca", objeto.marca);
                    cmd.Parameters.AddWithValue("placa", objeto.placa);
                    cmd.Parameters.AddWithValue("eje", objeto.eje);
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


        public bool Modificar(Vehiculodet objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarVehiculodet", oConexion);
                    cmd.Parameters.AddWithValue("iddet", objeto.iddet);
                    cmd.Parameters.AddWithValue("id", objeto.oVehiculo.id);
                    cmd.Parameters.AddWithValue("unidad", objeto.unidad);
                    cmd.Parameters.AddWithValue("marca", objeto.marca);
                    cmd.Parameters.AddWithValue("placa", objeto.placa);
                    cmd.Parameters.AddWithValue("eje", objeto.eje);
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




        public List<Vehiculodet> Listar()
        {

            List<Vehiculodet> rptListaVehiculodet = new List<Vehiculodet>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select v.iddet, v.id, b.tipo, v.unidad, v.marca, v.placa, v.eje, v.estado");
                sb.AppendLine("from VEHICULODET v");
                sb.AppendLine("left join VEHICULO b on b.id = v.id");
                sb.AppendLine("where v.estado = 1");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaVehiculodet.Add(new Vehiculodet()
                        {
                            iddet = Convert.ToInt32(dr["iddet"].ToString()),

                            oVehiculo = new Vehiculo() { 
                                id = Convert.ToInt32(dr["id"].ToString()), 
                                tipo = dr["tipo"].ToString() 
                            },

                            unidad = dr["unidad"].ToString(),
                            marca = dr["marca"].ToString(),
                            placa = dr["placa"].ToString(),
                            eje = Convert.ToInt32(dr["eje"].ToString()),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaVehiculodet;

                }
                catch (Exception ex)
                {
                    rptListaVehiculodet = null;
                    return rptListaVehiculodet;
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
                    SqlCommand cmd = new SqlCommand("delete from VEHICULODET where iddet = @iddet", oConexion);
                    cmd.Parameters.AddWithValue("@iddet", id);
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