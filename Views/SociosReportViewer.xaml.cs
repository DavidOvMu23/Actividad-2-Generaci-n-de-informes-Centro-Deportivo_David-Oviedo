using System.Windows;
using ModelView;

namespace Views
{
    /// <summary>
    /// Interaction logic for SociosReportViewer.xaml
    /// </summary>
    public partial class SociosReportViewer : Window
    {
        public SociosReportViewer()
        {
            InitializeComponent();

            // Creamos el ViewModel que provee los datos para el reporte de socios.
            var vm = new SociosReportViewModel();

            // Asignamos el DataContext para permitir binding en la vista si se requiere.
            DataContext = vm;

            // Creamos el objeto de reporte (Crystal Reports) y le pasamos la fuente de datos.
            var rpt = new SociosReport();
            rpt.SetDataSource(vm.SociosData);

            // Asignamos el reporte al control visor para mostrarlo en pantalla.
            reportViewer.ViewerCore.ReportSource = rpt;
        }
    }
}
