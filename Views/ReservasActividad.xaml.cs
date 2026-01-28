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
            // Obtenemos la lista de actividades desde el ViewModel y la asignamos
            // al combo para que el usuario pueda seleccionar una.
            DataTable dt = _viewModel.ObtenerActividades();
            comboActividades.ItemsSource = dt.DefaultView;
            comboActividades.DisplayMemberPath = "Nombre";
            comboActividades.SelectedValuePath = "Id";
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            // Si no se ha seleccionado ninguna actividad mostramos mensaje y no continuamos.
            if (comboActividades.SelectedValue == null)
            {
                MessageBox.Show("Seleccione una actividad.");
                return;
            }
            string idActividad = comboActividades.SelectedValue.ToString();

            // Abrimos la ventana del visor del reporte para la actividad seleccionada.
            var reportViewer = new ReservasActividadViewer(idActividad);
            reportViewer.Show();
            // Cerramos la ventana actual para dejar solo el visor abierto.
            this.Close();
        }
    }
}
