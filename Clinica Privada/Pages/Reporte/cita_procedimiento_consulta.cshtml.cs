using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Reporte.medicamento_consultaModel;

namespace Clinica_Privada.Pages.Reporte
{
    public class cita_procedimiento_consultaModel : PageModel
    {
        public List<CitaProcedimientoConsultaInfo> listaCitasProcedimientos = new List<CitaProcedimientoConsultaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            conexion.abrir();
            string script = "SELECT nombre, apellido_1, apellido_2, cantidad_padecimientos, cantidad_citas_recibidas, cantidad_procedimientos_recibidos, cantidad_medicamentos_recibidos, fecha_ultima_cita FROM VistaInformacionPacientes";
            MySqlCommand command = conexion.obtenerComando(script);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CitaProcedimientoConsultaInfo citaProcedimiento = new CitaProcedimientoConsultaInfo();
                    citaProcedimiento.nombre = reader.GetString(0);
                    citaProcedimiento.apellido1 = reader.GetString(1);
                    citaProcedimiento.apellido2 = reader.GetString(2);
                    citaProcedimiento.cantidad_padecimientos = "" + reader.GetInt32(3);
                    citaProcedimiento.cantidad_citas = "" + reader.GetInt32(4);
                    citaProcedimiento.cantidad_procedimientos = "" + reader.GetInt32(5);
                    citaProcedimiento.cantidad_medicamentos = "" + reader.GetInt32(6);
                    DateTime fechaHora = reader.GetDateTime(7);
                    citaProcedimiento.ultima_cita = fechaHora.ToString("yyyy-MM-dd-HH:mm");

                    listaCitasProcedimientos.Add(citaProcedimiento);
                }
            }
        }

        public class CitaProcedimientoConsultaInfo 
        {
            public string nombre { get; set; }
            public string apellido1 { get; set; }
            public string apellido2 { get; set; }
            public string cantidad_padecimientos { get; set; }
            public string cantidad_citas { get; set; }
            public string cantidad_procedimientos { get; set; }
            public string cantidad_medicamentos { get; set; }
            public string ultima_cita { get; set; }
        }
    }
}