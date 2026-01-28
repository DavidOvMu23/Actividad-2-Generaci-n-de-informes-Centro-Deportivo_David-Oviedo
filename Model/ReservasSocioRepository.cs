using System;
using System.Data;
using System.Data.SqlClient;

namespace Model
{
    public class ReservasSocioRepository
    {
        private readonly string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";

        public DataSetReservasActividad ObtenerHistorialReservasPorSocio(int? idSocio = null)
        {
            var ds = new DataSetReservasActividad();

           
            string sqlReservas = @"
                SELECT Id, SocioId, ActividadId, Fecha
                FROM Reserva
                WHERE (@SocioId IS NULL OR SocioId = @SocioId)
                ORDER BY Fecha ASC";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlReservas, cn))
            {
                cmd.Parameters.AddWithValue("@SocioId", idSocio.HasValue ? (object)idSocio.Value : DBNull.Value);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var r = ds.Reserva.NewReservaRow();
                        r.Id = dr["Id"].ToString();
                        r.SocioId = dr["SocioId"].ToString();
                        r.ActividadId = dr["ActividadId"].ToString();
                        r.Fecha = dr["Fecha"] == DBNull.Value ? string.Empty : dr["Fecha"].ToString();
                        ds.Reserva.AddReservaRow(r);
                    }
                }
                cn.Close();
            }

            
            string sqlSocios = @"
                SELECT DISTINCT s.Id, s.Nombre, s.Email, s.Activo
                FROM Socio s
                INNER JOIN Reserva r ON s.Id = r.SocioId
                WHERE (@SocioId IS NULL OR r.SocioId = @SocioId)";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlSocios, cn))
            {
                cmd.Parameters.AddWithValue("@SocioId", idSocio.HasValue ? (object)idSocio.Value : DBNull.Value);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var s = ds.Socio.NewSocioRow();
                        s.Id = dr["Id"].ToString();
                        s.Nombre = dr["Nombre"].ToString();
                        s.Email = dr["Email"] == DBNull.Value ? string.Empty : dr["Email"].ToString();
                        s.Activo = dr["Activo"] == DBNull.Value ? string.Empty : dr["Activo"].ToString();
                        ds.Socio.AddSocioRow(s);
                    }
                }
                cn.Close();
            }

           
            string sqlActividades = @"
                SELECT DISTINCT a.Id, a.Nombre, a.AforoMaximo
                FROM Actividad a
                INNER JOIN Reserva r ON a.Id = r.ActividadId
                WHERE (@SocioId IS NULL OR r.SocioId = @SocioId)";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlActividades, cn))
            {
                cmd.Parameters.AddWithValue("@SocioId", idSocio.HasValue ? (object)idSocio.Value : DBNull.Value);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var a = ds.Actividad.NewActividadRow();
                        a.Id = dr["Id"].ToString();
                        a.Nombre = dr["Nombre"].ToString();
                        a.AforoMaximo = dr["AforoMaximo"] == DBNull.Value ? string.Empty : dr["AforoMaximo"].ToString();
                        ds.Actividad.AddActividadRow(a);
                    }
                }
                cn.Close();
            }

            return ds;
        }
    }
}
