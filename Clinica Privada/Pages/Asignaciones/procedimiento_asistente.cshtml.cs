using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Asignaciones.cita_tratamientoModel;

namespace Clinica_Privada.Pages.Asignaciones
{
    public class procedimiento_asistenteModel : PageModel
    {
        public Procedimiento_asistenteInfo Procedimiento_asistente { get; set; } = new Procedimiento_asistenteInfo(); // Lista que guarda toda la información de los requests
        public ConexionBD conexion = new ConexionBD(); // Instancia de la clase Conexion para manejar la conexión a la base de datos
        public List<string> listaProcedimientos { get; set; } = new List<string>();
        public List<string> listaAsistentes { get; set; } = new List<string>();
        public string mensaje_error = ""; // Variable para almacenar mensajes de error
        public string mensaje_exito = ""; // Variable para almacenar mensajes de éxito

        /// <summary>
        /// Método que se ejecuta cuando se despliega los combobox para el ID de los procedimientos y las cedulas de los asistentes
        /// Objetivo: Extraer los datos de la base.
        /// Entradas: None
        /// Salidas: ID de procedimientos y cedulas de los asistentes
        /// </summary>
        public void OnGet()
        {
            conexion.abrir();
            string script_cita = "SELECT ID_procedimiento FROM Procedimiento";
            MySqlCommand command_paciente = conexion.obtenerComando(script_cita);

            using (MySqlDataReader reader = command_paciente.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ID = "" + reader.GetInt32(0);
                    listaProcedimientos.Add(ID);
                }
            }
            conexion.cerrar();

            conexion.abrir();
            string script_tratamiento = "SELECT cedula_medico FROM Personal_Medico";
            MySqlCommand command_medico = conexion.obtenerComando(script_tratamiento);

            using (MySqlDataReader reader = command_medico.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ID = "" + reader.GetInt32(0);
                    listaAsistentes.Add(ID);
                }
            }
            conexion.cerrar();
        }

        /// <summary>
        /// Método que se ejecuta cuando se envía el formulario (POST request).
        /// Objetivo: Recibir los datos del formulario de procedimiento - tratamiento, insertarlos en la base de datos y manejar errores.
        /// Entradas: Datos del formulario (ID_procedimiento, ID_tratamiento).
        /// Salidas: Mensaje de éxito o mensaje de error.
        /// Restricciones: Todos los campos deben estar debidamente validados antes de enviarse.
        /// </summary>
        public void onPost() 
        {
            Procedimiento_asistente.ID_procedimiento = Request.Form["id_procedimiento"];
            Procedimiento_asistente.cedula_asistente = Request.Form["cedula_asistente"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Asistente_Procedimiento (ID_procedimiento, personal_asistente)
                    VALUES (@ID_procedimiento, @cedula_asistente)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_procedimiento", Procedimiento_asistente.ID_procedimiento);
                command.Parameters.AddWithValue("@cedula_asistente", Procedimiento_asistente.cedula_asistente);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Procedimiento_asistente.ID_procedimiento = "";
                Procedimiento_asistente.cedula_asistente = "";

                mensaje_exito = "Formulario registrado exitosamente";
                conexion.cerrar();
            }
            catch (Exception ex)
            {
                mensaje_error = ex.Message;
                conexion.cerrar();
                OnGet();
            }
        }

        public class Procedimiento_asistenteInfo 
        {
            public string ID_procedimiento { get; set; }
            public string cedula_asistente { get; set; }
        }
    }
}