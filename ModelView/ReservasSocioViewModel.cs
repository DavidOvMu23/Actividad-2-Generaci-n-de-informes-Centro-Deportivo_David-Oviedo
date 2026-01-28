using Model;
using System.Data;

namespace ModelView
{
    public class ReservasSocioViewModel
    {
        private readonly ReservasSocioRepository _repo = new ReservasSocioRepository();

        public DataSet HistorialDataSet { get; private set; }

        public ReservasSocioViewModel(int? idSocio = null)
        {
            // Obtenemos el DataSet con el historial de reservas
            HistorialDataSet = _repo.ObtenerHistorialReservasPorSocio(idSocio);

            if (HistorialDataSet == null)
                return;

            // Verificamos que existan las tablas necesarias
            if (!HistorialDataSet.Tables.Contains("Reserva") || !HistorialDataSet.Tables.Contains("Socio"))
                return;

            var reservasTable = HistorialDataSet.Tables["Reserva"];
            var sociosTable = HistorialDataSet.Tables["Socio"];

            // Intentamos crear relaciones entre tablas para facilitar navegación en el DataSet.
            // Lo he hecho de esta forma por que haciendo las relaciones SOLO en el dataset me daba error
            // pero al hacerlo aquí y en el dataset si me ha ido, me ha ayudado el chat
            try
            {
                if (!HistorialDataSet.Relations.Contains("Socio_Reserva") && sociosTable.Columns.Contains("Id") && reservasTable.Columns.Contains("SocioId"))
                {
                    var rel = new DataRelation("Socio_Reserva", sociosTable.Columns["Id"], reservasTable.Columns["SocioId"]);
                    HistorialDataSet.Relations.Add(rel);
                }

                // Si existe la tabla Actividad, también creamos la relación con Reserva.
                if (!HistorialDataSet.Tables.Contains("Actividad") || !HistorialDataSet.Tables.Contains("Reserva"))
                {
                    // No hay actividad o no hay reservas, no hacemos nada.
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
                // Si cualquier creación de relación falla, ignoramos para no detener la carga del reporte.
            }

            // Construimos un resumen con el total de reservas por socio sin usar LINQ.
            var conteo = new System.Collections.Generic.Dictionary<string, int>();

            // Recorremos las reservas y contamos por SocioId.
            foreach (System.Data.DataRow fila in reservasTable.Rows)
            {
                var socioIdObj = fila["SocioId"];
                string socioId = socioIdObj == null || socioIdObj == System.DBNull.Value ? string.Empty : socioIdObj.ToString();
                if (string.IsNullOrEmpty(socioId))
                    continue;

                if (conteo.ContainsKey(socioId))
                    conteo[socioId] = conteo[socioId] + 1;
                else
                    conteo[socioId] = 1;
            }

            var dtResumen = new DataTable("ReservasPorSocio");
            dtResumen.Columns.Add("SocioId", typeof(string));
            dtResumen.Columns.Add("Nombre", typeof(string));
            dtResumen.Columns.Add("TotalReservas", typeof(int));

            // Para cada entrada en el diccionario buscamos el nombre del socio iterando la tabla Socio.
            foreach (var par in conteo)
            {
                string id = par.Key;
                int total = par.Value;

                string nombre = string.Empty;
                foreach (System.Data.DataRow sRow in sociosTable.Rows)
                {
                    var idObj = sRow["Id"];
                    string idSocioTabla = idObj == null || idObj == System.DBNull.Value ? string.Empty : idObj.ToString();
                    if (idSocioTabla == id)
                    {
                        var nomObj = sRow["Nombre"];
                        nombre = nomObj == null || nomObj == System.DBNull.Value ? string.Empty : nomObj.ToString();
                        break;
                    }
                }

                dtResumen.Rows.Add(id, nombre, total);
            }

            // Reemplazamos cualquier tabla previa de resumen y añadimos la nueva.
            if (HistorialDataSet.Tables.Contains("ReservasPorSocio"))
                HistorialDataSet.Tables.Remove("ReservasPorSocio");

            HistorialDataSet.Tables.Add(dtResumen);
        }
    }
}
