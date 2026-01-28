using ModelView;
using SAPBusinessObjects.WPF.Viewer;
using System.Data;
using System.Windows;
using Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for ReservasActividadViewer.xaml
    /// </summary>
    public partial class ReservasActividadViewer : Window
    {
        public ReservasActividadViewer(string idActividad)
        {
            InitializeComponent();

            // ViewModel que obtiene los datos del repositorio.
            var vm = new ReservasActividadViewModel();

            // Validamos que el id de actividad sea un entero válido.
            // Si la conversión falla mostramos un mensaje y salimos.
            int id;
            if (!int.TryParse(idActividad, out id))
            {
                MessageBox.Show("Id de actividad no válido.");
                Close();
                return;
            }

            // Obtenemos el DataSet con las reservas de la actividad.
            var dataSet = vm.ObtenerReservasPorActividad(id);

            // Comprobamos si el DataSet contiene la tabla Reserva y si tiene filas.
            // Si no hay reservas informamos al usuario y cerramos la ventana.
            if (dataSet == null || dataSet.Tables["Reserva"] == null || dataSet.Tables["Reserva"].Rows.Count == 0)
            {
                MessageBox.Show("No hay reservas para la actividad seleccionada.");
                Close();
                return;
            }

            // Creamos el reporte y le asignamos la fuente de datos.
            var report = new ReservasActividadReport();
            report.SetDataSource((System.Data.DataSet)dataSet);

            // Si el reporte tiene parámetros, los llenamos (ejemplo: ActividadId).
            if (report.ParameterFields.Count > 0)
            {
                report.SetParameterValue("ActividadId", id);
            }

            // Asignamos el reporte al visor para mostrarlo.
            crystalReportsViewer.ViewerCore.ReportSource = report;
        }
    }
}
