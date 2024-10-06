using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Clinica_Privada.Pages.Cita
{
    public class Cita_formModel : PageModel
    {
        public CitaInfo Cita { get; set; } = new CitaInfo(); // Lista que guarda toda la información de los requests
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
        /// Objetivo: Recibir los datos del formulario de cita, insertarlos en la base de datos y manejar errores.
        /// Entradas: Datos del formulario (id, fecha_hora, motivo, duracion, resultado, cedula_paciente, cedula_medico).
        /// Salidas: Mensaje de éxito o mensaje de error.
        /// Restricciones: Todos los campos deben estar debidamente validados antes de enviarse.
        /// </summary>
        public void OnPost()
        {
            Cita.ID = Request.Form["id"];
            Cita.fecha_hora = Request.Form["fecha_hora"];
            Cita.motivo = Request.Form["motivo"];
            Cita.duracion = Request.Form["duracion"];
            Cita.resultado = Request.Form["resultado"];
            Cita.cedula_paciente = Request.Form["cedula_paciente"];
            Cita.cedula_medico = Request.Form["cedula_medico"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Cita (ID_cita, fecha_hora, motivo, duracion, resultado, cedula_paciente, cedula_medico)
                    VALUES (@ID_cita, @fecha_hora, @motivo, @duracion, @resultado, @cedula_paciente, @cedula_medico)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_cita", Cita.ID);
                command.Parameters.AddWithValue("@fecha_hora", Cita.fecha_hora);
                command.Parameters.AddWithValue("@motivo", Cita.motivo);
                command.Parameters.AddWithValue("@duracion", Cita.duracion);
                command.Parameters.AddWithValue("@resultado", Cita.resultado);
                command.Parameters.AddWithValue("@cedula_paciente", Cita.cedula_paciente);
                command.Parameters.AddWithValue("@cedula_medico", Cita.cedula_medico);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Cita.ID = "";
                Cita.fecha_hora = "";
                Cita.motivo = "";
                Cita.duracion = "";
                Cita.resultado = "";
                Cita.cedula_medico = "";
                Cita.cedula_paciente = "";

                mensaje_exito = "Actividad registrada exitosamente";
                conexion.cerrar();
            }
            catch (Exception ex)
            {
                mensaje_error = ex.Message;
                conexion.cerrar();
            }
        }

        // Clase que representa el modelo de los datos de Cita
        public class CitaInfo {
            public string ID { get; set; }
            public string fecha_hora { get; set; }
            public string motivo { get; set; }
            public string duracion { get; set; }
            public string resultado { get; set; }
            public string cedula_paciente { get; set; }
            public string cedula_medico { get; set; }
        }
    }
}
