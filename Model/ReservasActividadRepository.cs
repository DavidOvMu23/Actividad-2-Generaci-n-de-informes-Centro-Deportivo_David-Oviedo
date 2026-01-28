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

            string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";

            
            string sqlActividad = @"SELECT Id, Nombre, AforoMaximo FROM Actividad WHERE Id = @IdActividad";
            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sqlActividad, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var row = ds.Actividad.NewActividadRow();
                        row.Id = dr["Id"].ToString();
                        row.Nombre = dr["Nombre"].ToString();
                        row.AforoMaximo = dr["AforoMaximo"] == DBNull.Value ? string.Empty : dr["AforoMaximo"].ToString();
                        ds.Actividad.AddActividadRow(row);
                    }
                }
                cn.Close();
            }

            
            string sqlReservas = @"SELECT Id, SocioId, ActividadId, Fecha FROM Reserva WHERE ActividadId = @IdActividad";
            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sqlReservas, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var row = ds.Reserva.NewReservaRow();
                        row.Id = dr["Id"].ToString();
                        row.SocioId = dr["SocioId"].ToString();
                        row.ActividadId = dr["ActividadId"].ToString();
                        row.Fecha = dr["Fecha"] == DBNull.Value ? string.Empty : dr["Fecha"].ToString();
                        ds.Reserva.AddReservaRow(row);
                    }
                }
                cn.Close();
            }

            
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
                    while (dr.Read())
                    {
                        var row = ds.Socio.NewSocioRow();
                        row.Id = dr["Id"].ToString();
                        row.Nombre = dr["Nombre"].ToString();
                        row.Email = dr["Email"] == DBNull.Value ? string.Empty : dr["Email"].ToString();
                        row.Activo = dr["Activo"] == DBNull.Value ? string.Empty : dr["Activo"].ToString();
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
