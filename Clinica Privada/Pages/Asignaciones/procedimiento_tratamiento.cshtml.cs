using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Asignaciones.cita_tratamientoModel;

namespace Clinica_Privada.Pages.Asignaciones
{
    public class procedimiento_tratamientoModel : PageModel
    {
        public Procedimiento_tratamientoInfo Procedimiento_tratamiento { get; set; } = new Procedimiento_tratamientoInfo(); // Lista que guarda toda la información de los requests
        public ConexionBD conexion = new ConexionBD(); // Instancia de la clase Conexion para manejar la conexión a la base de datos
        public List<string> listaProcedimientos { get; set; } = new List<string>();
        public List<string> listaTratamientos { get; set; } = new List<string>();
        public string mensaje_error = ""; // Variable para almacenar mensajes de error
        public string mensaje_exito = ""; // Variable para almacenar mensajes de éxito

        /// <summary>
        /// Método que se ejecuta cuando se despliega los combobox para los ID de los procedimientos y tratamientos.
        /// Objetivo: Extraer los datos de la base.
        /// Entradas: None
        /// Salidas: ID de procedimientos y tratamientos
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
            string script_tratamiento = "SELECT ID_tratamiento FROM Tratamiento";
            MySqlCommand command_medico = conexion.obtenerComando(script_tratamiento);

            using (MySqlDataReader reader = command_medico.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ID = "" + reader.GetInt32(0);
                    listaTratamientos.Add(ID);
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
            Procedimiento_tratamiento.ID_procedimiento = Request.Form["id_procedimiento"];
            Procedimiento_tratamiento.ID_tratamiento = Request.Form["id_tratamiento"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Procedimiento_tratamientos (ID_procedimiento, ID_tratamiento)
                    VALUES (@ID_procedimiento, @ID_tratamiento)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_procedimiento", Procedimiento_tratamiento.ID_procedimiento);
                command.Parameters.AddWithValue("@ID_tratamiento", Procedimiento_tratamiento.ID_tratamiento);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Procedimiento_tratamiento.ID_procedimiento = "";
                Procedimiento_tratamiento.ID_tratamiento = "";

                mensaje_exito = "Formulario registrado exitosamente";
                conexion.cerrar();
                OnGet();
            }
            catch (Exception ex)
            {
                mensaje_error = ex.Message;
                conexion.cerrar();
                OnGet();
            }
        }

        // Clase que representa el modelo de los datos de Procedimiento_tratamiento
        public class Procedimiento_tratamientoInfo
        {
            public string ID_procedimiento { get; set; }
            public string ID_tratamiento { get; set; }
        }
    }
}
