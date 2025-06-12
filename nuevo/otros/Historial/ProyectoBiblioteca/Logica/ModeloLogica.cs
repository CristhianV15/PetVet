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
    public class ModeloLogica
    {
        private static ModeloLogica instancia = null;

        public ModeloLogica() {

        }

        public static ModeloLogica Instancia {
            get {
                if (instancia == null) {
                    instancia = new ModeloLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Modelo oModelo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarModelo", oConexion);
                    cmd.Parameters.AddWithValue("modelo", oModelo.modelo);
                    cmd.Parameters.AddWithValue("precio", oModelo.precio);
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

        public bool Modificar(Modelo oModelo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarModelo", oConexion);
                    cmd.Parameters.AddWithValue("idmodelo", oModelo.idmodelo);
                    cmd.Parameters.AddWithValue("modelo", oModelo.modelo);
                    cmd.Parameters.AddWithValue("precio", oModelo.precio);
                    cmd.Parameters.AddWithValue("estado", oModelo.estado);
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


        public List<Modelo> Listar()
        {

            List<Modelo> rptListaModelo = new List<Modelo>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select idmodelo,modelo,precio,estado");
                sb.AppendLine("from MODELO");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaModelo.Add(new Modelo()
                        {
                            idmodelo = Convert.ToInt32(dr["idmodelo"].ToString()),
                            modelo = dr["modelo"].ToString(),
                            precio = Convert.ToDecimal(dr["precio"].ToString()),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaModelo;

                }
                catch (Exception ex)
                {
                    rptListaModelo = null;
                    return rptListaModelo;
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
                    SqlCommand cmd = new SqlCommand("delete from MODELO where idmodelo = @idmodelo", oConexion);
                    cmd.Parameters.AddWithValue("@idmodelo", id);
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