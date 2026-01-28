using System;
using System.Data;
using System.Data.SqlClient;

namespace Model
{
    public class ReservasSocioRepository
    {
        // Cadena de conexión a la base de datos.
        private readonly string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";

        public DataSetReservasActividad ObtenerHistorialReservasPorSocio(int? idSocio = null)
        {
            var ds = new DataSetReservasActividad();

            // Creamos el DataSet tipado que contendrá tablas Reserva, Socio y Actividad.

            // Consulta para obtener las reservas del (o de todos los) socio(s).
            // Si idSocio es null, se devuelven todas las reservas.
            string sqlReservas = @"
                SELECT Id, SocioId, ActividadId, Fecha
                FROM Reserva
                WHERE (@SocioId IS NULL OR SocioId = @SocioId)
                ORDER BY Fecha ASC";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlReservas, cn))
            {
                // Preparar parámetro @SocioId: si se proporcionó idSocio lo usamos,
                // en caso contrario usamos DBNull para indicar NULL en la consulta.
                object paramSocioId = DBNull.Value;
                if (idSocio.HasValue)
                {
                    paramSocioId = idSocio.Value;
                }
                cmd.Parameters.AddWithValue("@SocioId", paramSocioId);

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    // Leemos cada fila del reader y la convertimos a una fila del DataSet.
                    while (dr.Read())
                    {
                        var r = ds.Reserva.NewReservaRow();
                        r.Id = dr["Id"].ToString();
                        r.SocioId = dr["SocioId"].ToString();
                        r.ActividadId = dr["ActividadId"].ToString();

                        // Fecha puede ser nula en la BD, manejamos DBNull con if/else
                        object fechaObj = dr["Fecha"];
                        if (fechaObj == DBNull.Value)
                        {
                            r.Fecha = string.Empty;
                        }
                        else
                        {
                            r.Fecha = fechaObj.ToString();
                        }

                        ds.Reserva.AddReservaRow(r);
                    }
                }
                cn.Close();
            }

            
            // Consulta para obtener los socios que tienen reservas
            string sqlSocios = @"
                SELECT DISTINCT s.Id, s.Nombre, s.Email, s.Activo
                FROM Socio s
                INNER JOIN Reserva r ON s.Id = r.SocioId
                WHERE (@SocioId IS NULL OR r.SocioId = @SocioId)";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlSocios, cn))
            {
                // Preparar parámetro @SocioId para la consulta de socios: usar valor o DBNull.
                object paramSocioId2 = DBNull.Value;
                if (idSocio.HasValue)
                {
                    paramSocioId2 = idSocio.Value;
                }
                cmd.Parameters.AddWithValue("@SocioId", paramSocioId2);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    // Convertimos los socios devueltos por la consulta al DataSet.
                    while (dr.Read())
                    {
                        var s = ds.Socio.NewSocioRow();
                        s.Id = dr["Id"].ToString();
                        s.Nombre = dr["Nombre"].ToString();

                        object emailObj = dr["Email"];
                        if (emailObj == DBNull.Value)
                        {
                            s.Email = string.Empty;
                        }
                        else
                        {
                            s.Email = emailObj.ToString();
                        }

                        object activoObj = dr["Activo"];
                        if (activoObj == DBNull.Value)
                        {
                            s.Activo = string.Empty;
                        }
                        else
                        {
                            s.Activo = activoObj.ToString();
                        }

                        ds.Socio.AddSocioRow(s);
                    }
                }
                cn.Close();
            }

           
            // Consulta para obtener las actividades relacionadas con las reservas del socio
            string sqlActividades = @"
                SELECT DISTINCT a.Id, a.Nombre, a.AforoMaximo
                FROM Actividad a
                INNER JOIN Reserva r ON a.Id = r.ActividadId
                WHERE (@SocioId IS NULL OR r.SocioId = @SocioId)";

            using (var cn = new SqlConnection(conexion))
            using (var cmd = new SqlCommand(sqlActividades, cn))
            {
                // Preparar parámetro @SocioId para la consulta de actividades: usar valor o DBNull.
                object paramSocioId3 = DBNull.Value;
                if (idSocio.HasValue)
                {
                    paramSocioId3 = idSocio.Value;
                }
                cmd.Parameters.AddWithValue("@SocioId", paramSocioId3);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    // Rellenamos la tabla Actividad del DataSet.
                    while (dr.Read())
                    {
                        var a = ds.Actividad.NewActividadRow();
                        a.Id = dr["Id"].ToString();
                        a.Nombre = dr["Nombre"].ToString();

                        object aforoObj = dr["AforoMaximo"];
                        if (aforoObj == DBNull.Value)
                        {
                            a.AforoMaximo = string.Empty;
                        }
                        else
                        {
                            a.AforoMaximo = aforoObj.ToString();
                        }

                        ds.Actividad.AddActividadRow(a);
                    }
                }
                cn.Close();
            }

            return ds;
        }
    }
}
