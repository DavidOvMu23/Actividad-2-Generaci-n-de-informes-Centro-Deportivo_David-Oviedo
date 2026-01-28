using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelView
{
    public class ReservasActividadViewModel
    {
        private readonly ReservasActividadRepository _repo = new ReservasActividadRepository();

        public System.Data.DataSet ObtenerReservasPorActividad(int idActividad)
        {
            
            return _repo.ObtenerReservasPorActividad(idActividad);
        }

        public DataTable ObtenerActividades()
        {
            return _repo.ObtenerActividades();
        }
    }
}
