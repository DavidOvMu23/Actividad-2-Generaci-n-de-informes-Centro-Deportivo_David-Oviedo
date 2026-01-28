using System;
using System.Data;
using System.Windows;
using ModelView;

namespace Views
{
    public partial class ReservasActividad : Window
    {
        private readonly ReservasActividadViewModel _viewModel = new ReservasActividadViewModel();

        public ReservasActividad()
        {
            InitializeComponent();
            CargarActividades();
        }

        private void CargarActividades()
        {
            DataTable dt = _viewModel.ObtenerActividades();
            comboActividades.ItemsSource = dt.DefaultView;
            comboActividades.DisplayMemberPath = "Nombre";
            comboActividades.SelectedValuePath = "Id";
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            if (comboActividades.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una actividad.");
                return;
            }
            string idActividad = comboActividades.SelectedValue.ToString();

            var reportViewer = new ReservasActividadViewer(idActividad);
            reportViewer.Show();
            this.Close();
        }
    }
}
