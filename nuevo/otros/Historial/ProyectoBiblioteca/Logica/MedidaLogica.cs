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
    public class MedidaLogica
    {
        private static MedidaLogica instancia = null;

        public MedidaLogica() {

        }

        public static MedidaLogica Instancia {
            get {
                if (instancia == null) {
                    instancia = new MedidaLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Medida oMedida)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMedida", oConexion);
                    cmd.Parameters.AddWithValue("medida", oMedida.medida);
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

        public bool Modificar(Medida oMedida)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarMedida", oConexion);
                    cmd.Parameters.AddWithValue("idmedida", oMedida.idmedida);
                    cmd.Parameters.AddWithValue("medida", oMedida.medida);
                    cmd.Parameters.AddWithValue("estado", oMedida.estado);
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


        public List<Medida> Listar()
        {

            List<Medida> rptListaMedida = new List<Medida>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select idmedida,medida,estado");
                sb.AppendLine("from MEDIDA");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaMedida.Add(new Medida()
                        {
                            idmedida = Convert.ToInt32(dr["idmedida"].ToString()),
                            medida = dr["medida"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaMedida;

                }
                catch (Exception ex)
                {
                    rptListaMedida = null;
                    return rptListaMedida;
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
                    SqlCommand cmd = new SqlCommand("delete from MEDIDA where idmedida = @idmedida", oConexion);
                    cmd.Parameters.AddWithValue("@idmedida", id);
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