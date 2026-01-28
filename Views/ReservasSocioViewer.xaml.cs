using System.Windows;
using ModelView;

namespace Views
{
    /// <summary>
    /// Interaction logic for SociosReportViewer.xaml
    /// </summary>
    public partial class ReservasSocioViewer : Window
    {
        public ReservasSocioViewer()
        {
            InitializeComponent();

            // Creamos el ViewModel que obtiene el historial de reservas por socio.
            var vm = new ReservasSocioViewModel();

            // Asignamos DataContext por si la vista requiere binding.
            DataContext = vm;

            // Creamos el reporte y le asignamos el DataSet obtenido del ViewModel.
            var rpt = new ReservasSocioReport();
            rpt.SetDataSource(vm.HistorialDataSet);

            // Asignamos el reporte al visor para mostrar el historial.
            reportViewer.ViewerCore.ReportSource = rpt;
        }
    }
}
