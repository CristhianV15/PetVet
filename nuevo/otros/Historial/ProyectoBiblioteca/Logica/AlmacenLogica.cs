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
    public class AlmacenLogica
    {
        private static AlmacenLogica instancia = null;

        public AlmacenLogica() {

        }

        public static AlmacenLogica Instancia {
            get {
                if (instancia == null) {
                    instancia = new AlmacenLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Almacen oAlmacen)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarAlmacen", oConexion);
                    cmd.Parameters.AddWithValue("almacen", oAlmacen.almacen);
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

        public bool Modificar(Almacen oAlmacen)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarAlmacen", oConexion);
                    cmd.Parameters.AddWithValue("idalmacen", oAlmacen.idalmacen);
                    cmd.Parameters.AddWithValue("almacen", oAlmacen.almacen);
                    cmd.Parameters.AddWithValue("estado", oAlmacen.estado);
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


        public List<Almacen> Listar()
        {

            List<Almacen> rptListaAlmacen = new List<Almacen>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select idalmacen,almacen,estado");
                sb.AppendLine("from ALMACEN");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaAlmacen.Add(new Almacen()
                        {
                            idalmacen = Convert.ToInt32(dr["idalmacen"].ToString()),
                            almacen = dr["almacen"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaAlmacen;

                }
                catch (Exception ex)
                {
                    rptListaAlmacen = null;
                    return rptListaAlmacen;
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
                    SqlCommand cmd = new SqlCommand("delete from ALMACEN where idalmacen = @idalmacen", oConexion);
                    cmd.Parameters.AddWithValue("@idalmacen", id);
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