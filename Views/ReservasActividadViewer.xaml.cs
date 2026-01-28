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

            
            var vm = new ReservasActividadViewModel();

            
            if (!int.TryParse(idActividad, out var id))
            {
                MessageBox.Show("Id de actividad no válido.");
                Close();
                return;
            }

            var dataSet = vm.ObtenerReservasPorActividad(id);

            if (dataSet == null || dataSet.Tables["Reserva"] == null || dataSet.Tables["Reserva"].Rows.Count == 0)
            {
                MessageBox.Show("No hay reservas para la actividad seleccionada.");
                Close();
                return;
            }

           
            var report = new ReservasActividadReport();
            
            report.SetDataSource((System.Data.DataSet)dataSet);

           
            if (report.ParameterFields.Count > 0)
            {
                report.SetParameterValue("ActividadId", id);
            }

            
            crystalReportsViewer.ViewerCore.ReportSource = report;
        }
    }
}
