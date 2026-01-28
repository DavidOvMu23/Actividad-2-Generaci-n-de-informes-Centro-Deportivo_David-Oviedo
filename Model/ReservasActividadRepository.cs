using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReservasActividadRepository
    {

        public DataSetReservasActividad ObtenerReservasPorActividad(int idActividad)
        {
            var ds = new DataSetReservasActividad();

            // Cadena de conexión a la base de datos.
            string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";

            
            // Consulta para obtener los datos de la actividad seleccionada.
            string sqlActividad = @"SELECT Id, Nombre, AforoMaximo FROM Actividad WHERE Id = @IdActividad";
            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sqlActividad, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Leemos las filas devueltas por la consulta de actividad.
                    // Por cada fila creamos una nueva fila en la tabla Actividad del DataSet.
                    while (dr.Read())
                    {
                        var row = ds.Actividad.NewActividadRow();
                        row.Id = dr["Id"].ToString();
                        row.Nombre = dr["Nombre"].ToString();

                        // Comprobamos si AforoMaximo es DBNull y lo asignamos en consecuencia.
                        object aforoObj = dr["AforoMaximo"];
                        if (aforoObj == DBNull.Value)
                        {
                            row.AforoMaximo = string.Empty;
                        }
                        else
                        {
                            row.AforoMaximo = aforoObj.ToString();
                        }

                        ds.Actividad.AddActividadRow(row);
                    }
                }
                cn.Close();
            }

            
            // Consulta para obtener las reservas de la actividad.
            string sqlReservas = @"SELECT Id, SocioId, ActividadId, Fecha FROM Reserva WHERE ActividadId = @IdActividad";
            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sqlReservas, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Leemos las reservas devueltas por la consulta.
                    // Cada fila se mapea a la tabla Reserva del DataSet.
                    while (dr.Read())
                    {
                        var row = ds.Reserva.NewReservaRow();
                        row.Id = dr["Id"].ToString();
                        row.SocioId = dr["SocioId"].ToString();
                        row.ActividadId = dr["ActividadId"].ToString();

                        // Fecha puede ser DBNull; comprobamos y asignamos valor apropiado.
                        object fechaObj = dr["Fecha"];
                        if (fechaObj == DBNull.Value)
                        {
                            row.Fecha = string.Empty;
                        }
                        else
                        {
                            row.Fecha = fechaObj.ToString();
                        }

                        ds.Reserva.AddReservaRow(row);
                    }
                }
                cn.Close();
            }

            
            // Consulta para obtener los socios que realizaron reserva en la actividad.
            string sqlSocios = @"
                SELECT DISTINCT s.Id, s.Nombre, s.Email, s.Activo
                FROM Socio s
                INNER JOIN Reserva r ON s.Id = r.SocioId
                WHERE r.ActividadId = @IdActividad";
            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sqlSocios, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // Rellenamos la tabla Socio del DataSet con los resultados.
                    while (dr.Read())
                    {
                        var row = ds.Socio.NewSocioRow();
                        row.Id = dr["Id"].ToString();
                        row.Nombre = dr["Nombre"].ToString();
                        object emailObj = dr["Email"];
                        if (emailObj == DBNull.Value)
                        {
                            row.Email = string.Empty;
                        }
                        else
                        {
                            row.Email = emailObj.ToString();
                        }

                        object activoObj = dr["Activo"];
                        if (activoObj == DBNull.Value)
                        {
                            row.Activo = string.Empty;
                        }
                        else
                        {
                            row.Activo = activoObj.ToString();
                        }
                        ds.Socio.AddSocioRow(row);
                    }
                }
                cn.Close();
            }

            return ds;
        }

        public DataTable ObtenerActividades()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));

            string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";
            string sql = @"SELECT Id, Nombre FROM Actividad";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dt.Rows.Add(dr["Id"], dr["Nombre"]);
                }
            }
            return dt;
        }
    }
}
