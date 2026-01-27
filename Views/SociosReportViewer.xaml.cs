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

            // 1. Instanciamos el ViewModel para obtener los datos
            var vm = new SociosReportViewModel();

            // 2. Asignamos el contexto de datos (opcional, por si usas bindings en otros lados)
            DataContext = vm;

            // 3. Instanciamos el reporte visual (Aquí SÍ es válido porque estamos en la Capa de Vista)
            var rpt = new SociosReport();

            // 4. Inyectamos los datos del ViewModel al Reporte
            // Nota: vm.SociosData es el DataSet que creamos en el Paso 1
            rpt.SetDataSource(vm.SociosData);

            // 5. Asignamos el reporte al visor visual
            reportViewer.ViewerCore.ReportSource = rpt;
        }
    }
}
