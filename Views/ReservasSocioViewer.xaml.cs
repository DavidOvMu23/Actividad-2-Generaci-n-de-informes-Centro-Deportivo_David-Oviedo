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

           
            var vm = new ReservasSocioViewModel();

            
            DataContext = vm;

           
            var rpt = new ReservasSocioReport();

           
            rpt.SetDataSource(vm.HistorialDataSet);

           
            reportViewer.ViewerCore.ReportSource = rpt;
        }
    }
}
