using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace Clinica_Privada.Pages.Reporte
{
    public class paciente_consultaModel : PageModel
    {
        public List<PacienteConsultaInfo> listaPacientes = new List<PacienteConsultaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            conexion.abrir();
            string script = "SELECT nombre, apellido_1, apellido_2, sexo, cantidad_atenciones FROM VistaPacientesAtendidos";
            MySqlCommand command = conexion.obtenerComando(script);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    PacienteConsultaInfo paciente = new PacienteConsultaInfo();
                    paciente.nombre = reader.GetString(0);
                    paciente.apellido1 = reader.GetString(1);
                    paciente.apellido2 = reader.GetString(2);
                    paciente.sexo = reader.GetString(3);
                    paciente.cantidadCitas = "" + reader.GetInt32(4);

                    listaPacientes.Add(paciente);
                }
            }
        }

        public class PacienteConsultaInfo
        {
            public string nombre { get; set; }
            public string apellido1 { get; set; }
            public string apellido2 { get; set; }
            public string sexo { get; set; }
            public string cantidadCitas { get; set; }
        }
    }
}
