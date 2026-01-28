using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace Views
{
    /// <summary>
    /// Lógica de interacción para Socios.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button_socios_Click(object sender, RoutedEventArgs e)
        {
            SociosReportViewer sociosReportViewer = new SociosReportViewer();
            sociosReportViewer.Show();
        }

        private void button_reservasActividad_Click(object sender, RoutedEventArgs e)
        {   ReservasActividad reservasActividad = new ReservasActividad();
            reservasActividad.Show();
     
        }

        private void button_reservasSocios_Click(object sender, RoutedEventArgs e)
        {
            ReservasSocioViewer reservasSocioViewer = new ReservasSocioViewer();
            reservasSocioViewer.Show();
        }

        public DataTable ObtenerReservasPorActividad(string nombreActividad)
        {
            var dt = new DataTable();
            dt.Columns.Add("NombreActividad", typeof(string));
            dt.Columns.Add("FechaReserva", typeof(DateTime));
            dt.Columns.Add("NombreSocio", typeof(string));
            dt.Columns.Add("AforoMaximo", typeof(int));

            // Construimos la tabla en memoria con las columnas esperadas por la vista.
            // Cadena de conexión a la BD.
            string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";
            // Consulta para obtener las reservas de una actividad por nombre.
            string sql = @"
                SELECT 
                    a.Nombre AS NombreActividad,
                    r.Fecha AS FechaReserva,
                    s.Nombre AS NombreSocio,
                    a.AforoMaximo
                FROM 
                    Reserva r
                    INNER JOIN Actividad a ON r.ActividadId = a.Id
                    INNER JOIN Socio s ON r.SocioId = s.Id
                WHERE 
                    a.Nombre = @NombreActividad";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@NombreActividad", nombreActividad);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                // Rellenamos la tabla con los resultados de la consulta.
                while (dr.Read())
                {
                    // Nombre actividad
                    string nombreAct = dr["NombreActividad"].ToString();

                    // FechaReserva: comprobamos DBNull
                    object fechaObj = dr["FechaReserva"];
                    object fechaVal;
                    if (fechaObj == DBNull.Value)
                    {
                        fechaVal = DBNull.Value;
                    }
                    else
                    {
                        fechaVal = Convert.ToDateTime(fechaObj);
                    }

                    // Nombre socio
                    string nombreSocio = dr["NombreSocio"].ToString();

                    // AforoMaximo: comprobamos DBNull
                    object aforoObj = dr["AforoMaximo"];
                    object aforoVal;
                    if (aforoObj == DBNull.Value)
                    {
                        aforoVal = DBNull.Value;
                    }
                    else
                    {
                        aforoVal = Convert.ToInt32(aforoObj);
                    }

                    dt.Rows.Add(nombreAct, fechaVal, nombreSocio, aforoVal);
                }
            }
            return dt;
        }
    }
}
