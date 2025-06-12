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
    public class VehiculoLogica
    {
        private static VehiculoLogica instancia = null;

        public VehiculoLogica() {

        }

        public static VehiculoLogica Instancia {
            get {
                if (instancia == null) {
                    instancia = new VehiculoLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Vehiculo oVehiculo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVehiculo", oConexion);
                    cmd.Parameters.AddWithValue("tipo", oVehiculo.tipo);
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

        public bool Modificar(Vehiculo oVehiculo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarVehiculo", oConexion);
                    cmd.Parameters.AddWithValue("id", oVehiculo.id);
                    cmd.Parameters.AddWithValue("tipo", oVehiculo.tipo);
                    cmd.Parameters.AddWithValue("estado", oVehiculo.estado);
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


        public List<Vehiculo> Listar()
        {

            List<Vehiculo> rptListaVehiculo = new List<Vehiculo>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select id,tipo,estado");
                sb.AppendLine("from VEHICULO");
                sb.AppendLine("where estado = 1");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaVehiculo.Add(new Vehiculo()
                        {
                            id = Convert.ToInt32(dr["id"].ToString()),
                            tipo = dr["tipo"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaVehiculo;

                }
                catch (Exception ex)
                {
                    rptListaVehiculo = null;
                    return rptListaVehiculo;
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
                    SqlCommand cmd = new SqlCommand("delete from VEHICULO where id = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
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