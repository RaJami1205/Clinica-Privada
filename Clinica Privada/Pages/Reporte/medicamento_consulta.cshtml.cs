using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Reporte.paciente_consultaModel;

namespace Clinica_Privada.Pages.Reporte
{
    public class medicamento_consultaModel : PageModel
    {
        public List<MedicamentoConsultaInfo> listaMedicamentos = new List<MedicamentoConsultaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            conexion.abrir();
            string script = "SELECT nombre_medicamento, anio, mes, cantidad_utilizada FROM VistaMedicamentosUtilizados";
            MySqlCommand command = conexion.obtenerComando(script);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    MedicamentoConsultaInfo medicamento = new MedicamentoConsultaInfo();
                    medicamento.nombre = reader.GetString(0);
                    medicamento.año = "" + reader.GetInt32(1);
                    medicamento.mes = "" + reader.GetInt32(2);
                    medicamento.cantidad = "" + reader.GetInt32(3);

                    listaMedicamentos.Add(medicamento);
                }
            }
        }

        // Clase que representa el modelo de los datos para la consulta del medicamento
        public class MedicamentoConsultaInfo 
        {
            public string nombre { get; set; }
            public string año { get; set; }
            public string mes { get; set; }
            public string cantidad { get; set; }
        }
    }
}
