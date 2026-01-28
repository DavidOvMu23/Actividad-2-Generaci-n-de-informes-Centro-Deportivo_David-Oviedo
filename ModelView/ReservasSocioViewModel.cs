using Model;
using System.Data;
using System.Linq;

namespace ModelView
{
    public class ReservasSocioViewModel
    {
        private readonly ReservasSocioRepository _repo = new ReservasSocioRepository();

        public DataSet HistorialDataSet { get; private set; }

        public ReservasSocioViewModel(int? idSocio = null)
        {
            HistorialDataSet = _repo.ObtenerHistorialReservasPorSocio(idSocio);
            
            if (HistorialDataSet == null)
                return;

            if (!HistorialDataSet.Tables.Contains("Reserva") || !HistorialDataSet.Tables.Contains("Socio"))
                return;

            var reservasTable = HistorialDataSet.Tables["Reserva"];
            var sociosTable = HistorialDataSet.Tables["Socio"];

           
            try
            {
                if (!HistorialDataSet.Relations.Contains("Socio_Reserva") && sociosTable.Columns.Contains("Id") && reservasTable.Columns.Contains("SocioId"))
                {
                    var rel = new DataRelation("Socio_Reserva", sociosTable.Columns["Id"], reservasTable.Columns["SocioId"]);
                    HistorialDataSet.Relations.Add(rel);
                }

                if (!HistorialDataSet.Tables.Contains("Actividad") || !HistorialDataSet.Tables.Contains("Reserva"))
                {
                    
                }
                else
                {
                    var actividadesTable = HistorialDataSet.Tables["Actividad"];
                    if (!HistorialDataSet.Relations.Contains("Actividad_Reserva") && actividadesTable.Columns.Contains("Id") && reservasTable.Columns.Contains("ActividadId"))
                    {
                        var rel2 = new DataRelation("Actividad_Reserva", actividadesTable.Columns["Id"], reservasTable.Columns["ActividadId"]);
                        HistorialDataSet.Relations.Add(rel2);
                    }
                }
            }
            catch
            {
                
            }

            var agrup = reservasTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("SocioId"))
                .Select(g => new
                {
                    SocioId = g.Key,
                    TotalReservas = g.Count()
                });

            var dtResumen = new DataTable("ReservasPorSocio");
            dtResumen.Columns.Add("SocioId", typeof(string));
            dtResumen.Columns.Add("Nombre", typeof(string));
            dtResumen.Columns.Add("TotalReservas", typeof(int));

            foreach (var item in agrup)
            {
                var nombre = sociosTable.AsEnumerable()
                    .Where(s => s.Field<string>("Id") == item.SocioId)
                    .Select(s => s.Field<string>("Nombre"))
                    .FirstOrDefault() ?? string.Empty;

                dtResumen.Rows.Add(item.SocioId, nombre, item.TotalReservas);
            }

            if (HistorialDataSet.Tables.Contains("ReservasPorSocio"))
                HistorialDataSet.Tables.Remove("ReservasPorSocio");

            HistorialDataSet.Tables.Add(dtResumen);
        }
    }
}
