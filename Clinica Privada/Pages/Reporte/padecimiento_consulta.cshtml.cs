using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Reporte.medicamento_consultaModel;

namespace Clinica_Privada.Pages.Reporte
{
    public class padecimiento_consultaModel : PageModel
    {
        public List<PadecimientoConsultaInfo> listaPadecimientos = new List<PadecimientoConsultaInfo>();
        public ConexionBD conexion = new ConexionBD();

        public void OnGet()
        {
            conexion.abrir();
            string script = "SELECT nombre_padecimiento, anio, cantidad_inicios FROM VistaCantidadPadecimientosPorAnio";
            MySqlCommand command = conexion.obtenerComando(script);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    PadecimientoConsultaInfo padecimiento = new PadecimientoConsultaInfo();
                    padecimiento.nombre = reader.GetString(0);
                    padecimiento.año = "" + reader.GetInt32(1);
                    padecimiento.cantidad = "" + reader.GetInt32(2);

                    listaPadecimientos.Add(padecimiento);
                }
            }
        }

        // Clase que representa el modelo de los datos para la consulta de padecimiento
        public class PadecimientoConsultaInfo 
        {
            public string nombre { get; set; }
            public string año { get; set; }
            public string cantidad { get; set; }
        }
    }
}