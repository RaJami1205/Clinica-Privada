using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Clinica_Privada.Pages.Procedimiento
{
    public class Procedimiento_listModel : PageModel
    {
        public List<ProcedimientoInfo> listaProcedimientos = new List<ProcedimientoInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            try
            {
                conexion.abrir();
                string script = "SELECT ID_procedimiento, cedula_medico_operacion, cedula_paciente, fecha, motivo FROM Procedimiento";
                MySqlCommand command = conexion.obtenerComando(script);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProcedimientoInfo procedimiento = new ProcedimientoInfo();
                        procedimiento.ID = "" + reader.GetInt32(0);
                        procedimiento.cedula_medico = "" + reader.GetInt32(1);
                        procedimiento.cedula_paciente = "" + reader.GetInt32(2);
                        DateTime fechaProcedimiento = reader.GetDateTime(3);
                        procedimiento.fecha = fechaProcedimiento.ToString("yyyy-MM-dd");
                        procedimiento.motivo = reader.GetString(4);

                        listaProcedimientos.Add(procedimiento);
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí se maneja el error
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Clase que representa el modelo de los datos de Procedimiento
        public class ProcedimientoInfo
        {
            public string ID { get; set; }
            public string fecha { get; set; }
            public string motivo { get; set; }
            public string cedula_medico { get; set; }
            public string cedula_paciente { get; set; }
        }
    }
}