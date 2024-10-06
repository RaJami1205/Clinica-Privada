using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Clinica_Privada.Pages.Cita
{
    public class Cita_listModel : PageModel
    {
        public List<CitaInfo> listaCitas = new List<CitaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            try
            {
                conexion.abrir();
                string script = "SELECT ID_cita, cedula_medico, cedula_paciente, fecha_hora, motivo,resultado FROM Cita";
                MySqlCommand command = conexion.obtenerComando(script);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CitaInfo cita = new CitaInfo();
                        cita.ID = "" + reader.GetInt32(0);
                        cita.cedula_medico = "" + reader.GetInt32(1);
                        cita.cedula_paciente = "" + reader.GetInt32(2);
                        DateTime fechaHora = reader.GetDateTime(3);
                        cita.fecha_hora = fechaHora.ToString("yyyy-MM-dd-HH:mm");
                        cita.motivo = reader.GetString(4);
                        cita.resultado = reader.GetString(5);

                        listaCitas.Add(cita);
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí se maneja el error
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Clase que representa el modelo de los datos de Cita
        public class CitaInfo
        {
            public string ID { get; set; }
            public string fecha_hora { get; set; }
            public string motivo { get; set; }
            public string resultado { get; set; }
            public string cedula_paciente { get; set; }
            public string cedula_medico { get; set; }
        }
    }
}