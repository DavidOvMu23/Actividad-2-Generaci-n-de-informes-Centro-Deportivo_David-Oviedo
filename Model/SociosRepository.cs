using System.Data;
using System.Data.SqlClient;

namespace Model
{
    public class SociosRepository
    {
        public DataTable ObtenerSocios()
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Estado", typeof(string));

            string conexion = "Server=localhost\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";
            string sql = @"SELECT Id AS ID, Nombre, Email, 
                                  CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
                           FROM Socio";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dt.Rows.Add(
                        dr["ID"],
                        dr["Nombre"],
                        dr["Email"],
                        dr["Estado"]
                    );
                }
            }
            return dt;
        }
    }
}