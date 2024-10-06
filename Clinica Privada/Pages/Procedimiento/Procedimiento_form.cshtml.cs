using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Clinica_Privada.Pages.Procedimiento
{
    public class Procedimiento_formModel : PageModel
    {

        public ProcedimientoInfo Procedimiento { get; set; } = new ProcedimientoInfo(); // Lista que guarda toda la información de los requests
        public ConexionBD conexion = new ConexionBD(); // Instancia de la clase Conexion para manejar la conexión a la base de datos
        public List<string> listaCedulaPaciente { get; set; } = new List<string>();
        public List<string> listaCedulaMedico { get; set; } = new List<string>();
        public string mensaje_error = ""; // Variable para almacenar mensajes de error
        public string mensaje_exito = ""; // Variable para almacenar mensajes de éxito

        /// <summary>
        /// Método que se ejecuta cuando se despliega los combobox para cedulas de pacientes y medicos.
        /// Objetivo: Extraer los datos de la base.
        /// Entradas: None
        /// Salidas: Cedulas de pacientes y de medicos
        /// </summary>
        public void OnGet()
        {
            conexion.abrir();
            string script_paciente = "SELECT cedula_paciente FROM Paciente";
            MySqlCommand command_paciente = conexion.obtenerComando(script_paciente);

            using (MySqlDataReader reader = command_paciente.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaCedulaPaciente.Add(reader.GetString(0));
                }
            }
            conexion.cerrar();

            conexion.abrir();
            string script_medico = "SELECT cedula_medico FROM Personal_Medico";
            MySqlCommand command_medico = conexion.obtenerComando(script_medico);

            using (MySqlDataReader reader = command_medico.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaCedulaMedico.Add(reader.GetString(0));
                }
            }
            conexion.cerrar();
        }

        /// <summary>
        /// Método que se ejecuta cuando se envía el formulario (POST request).
        /// Objetivo: Recibir los datos del formulario de procedimiento, insertarlos en la base de datos y manejar errores.
        /// Entradas: Datos del formulario (id, fecha_hora, motivo, epicrisis, cedula_paciente, cedula_medico).
        /// Salidas: Mensaje de éxito o mensaje de error.
        /// Restricciones: Todos los campos deben estar debidamente validados antes de enviarse.
        /// </summary>
        public void OnPost() 
        {
            Procedimiento.ID = Request.Form["id"];
            Procedimiento.fecha_hora = Request.Form["fecha"];
            Procedimiento.motivo = Request.Form["motivo"];
            Procedimiento.epicrisis = Request.Form["epicrisis"];
            Procedimiento.cedula_paciente = Request.Form["cedula_paciente"];
            Procedimiento.cedula_medico = Request.Form["cedula_medico"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Procedimiento (ID_procedimiento, fecha, motivo, epicrisis, cedula_paciente, cedula_medico_operacion)
                    VALUES (@ID_procedimiento, @fecha, @motivo, @epicrisis, @cedula_paciente, @cedula_medico)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_procedimiento", Procedimiento.ID);
                command.Parameters.AddWithValue("@fecha", Procedimiento.fecha_hora);
                command.Parameters.AddWithValue("@motivo", Procedimiento.motivo);
                command.Parameters.AddWithValue("@epicrisis", Procedimiento.epicrisis);
                command.Parameters.AddWithValue("@cedula_paciente", Procedimiento.cedula_paciente);
                command.Parameters.AddWithValue("@cedula_medico", Procedimiento.cedula_medico);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Procedimiento.ID = "";
                Procedimiento.fecha_hora = "";
                Procedimiento.motivo = "";
                Procedimiento.cedula_medico = "";
                Procedimiento.cedula_paciente = "";

                mensaje_exito = "Actividad registrada exitosamente";
                conexion.cerrar();
            }
            catch (Exception ex)
            {
                mensaje_error = ex.Message;
                conexion.cerrar();
                OnGet();
            }

        }

        // Clase que representa el modelo de los datos de Procedimiento
        public class ProcedimientoInfo
        {
            public string ID { get; set; }
            public string fecha_hora { get; set; }
            public string motivo { get; set; }
            public string epicrisis { get; set; }
            public string cedula_medico { get; set; }
            public string cedula_paciente { get; set; }
        }
    }
}
