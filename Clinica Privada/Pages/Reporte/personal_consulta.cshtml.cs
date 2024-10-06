using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Reporte.paciente_consultaModel;

namespace Clinica_Privada.Pages.Reporte
{
    public class personal_consultaModel : PageModel
    {
        public List<PersonalConsultaInfo> listaPersonales = new List<PersonalConsultaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            conexion.abrir();
            string script = "SELECT nombre, apellido_1, apellido_2, cantidad_citas_atendidas, cantidad_procedimientos_atendidos, cantidad_medicamentos_suministrados FROM VistaAtencionesPorPersonalMedico";
            MySqlCommand command = conexion.obtenerComando(script);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    PersonalConsultaInfo personal = new PersonalConsultaInfo();
                    personal.nombre = reader.GetString(0);
                    personal.apellido1 = reader.GetString(1);
                    personal.apellido2 = reader.GetString(2);
                    personal.cantidad_citas = "" + reader.GetInt32(3);
                    personal.cantidad_procedimientos = "" + reader.GetInt32(4);
                    personal.cantidad_medicamentos = "" + reader.GetInt32(5);

                    listaPersonales.Add(personal);
                }
            }
        }

        public class PersonalConsultaInfo 
        {
            public string nombre { get; set; }
            public string apellido1 { get; set; }
            public string apellido2 { get; set; }
            public string cantidad_citas { get; set; }
            public string cantidad_procedimientos { get; set; }
            public string cantidad_medicamentos { get; set; }
        }
    }
}
