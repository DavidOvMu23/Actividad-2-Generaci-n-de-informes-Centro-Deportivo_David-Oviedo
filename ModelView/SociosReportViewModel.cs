using Model;
using System.Data;

namespace ModelView
{
    public class SociosReportViewModel
    {
        private readonly SociosRepository _repo = new SociosRepository();

        // Propiedad que contendrá los datos que consume el reporte de socios.
        public DataTable SociosData { get; private set; }

        // Constructor: obtiene datos del repositorio al instanciar el ViewModel.
        public SociosReportViewModel()
        {
            // Llamada al repositorio para llenar la tabla con los socios.
            SociosData = _repo.ObtenerSocios();
        }
    }
}