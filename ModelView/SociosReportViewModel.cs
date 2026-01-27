using Model;
using System.Data;

namespace ModelView
{
    public class SociosReportViewModel
    {
        private readonly SociosRepository _repo = new SociosRepository();

        public DataTable SociosData { get; private set; }

        public SociosReportViewModel()
        {
            SociosData = _repo.ObtenerSocios();
        }
    }
}