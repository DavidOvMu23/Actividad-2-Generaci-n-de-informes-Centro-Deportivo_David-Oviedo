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
            this.Close();
        }

        private void button_reservasActividad_Click(object sender, RoutedEventArgs e)
        {
            ReservasPorActividad reservasPorActividad = new ReservasPorActividad();
            reservasPorActividad.Show();
            this.Close();
        }

        private void button_reservasSocios_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
