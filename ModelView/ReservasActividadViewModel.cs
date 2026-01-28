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
            // Devuelve un DataSet con tablas (Actividad, Reserva, Socio) para la actividad indicada.
            return _repo.ObtenerReservasPorActividad(idActividad);
        }

        public DataTable ObtenerActividades()
        {
            // Devuelve una tabla con las actividades para llenar un combo en la vista.
            return _repo.ObtenerActividades();
        }
    }
}
