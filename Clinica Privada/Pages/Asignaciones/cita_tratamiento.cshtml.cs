using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Clinica_Privada.Pages.Asignaciones
{
    public class cita_tratamientoModel : PageModel
    {
        public Cita_tratamientoInfo Cita_tratamiento { get; set; } = new Cita_tratamientoInfo(); // Lista que guarda toda la información de los requests
        public ConexionBD conexion = new ConexionBD(); // Instancia de la clase Conexion para manejar la conexión a la base de datos
        public List<string> listaCitas { get; set; } = new List<string>();
        public List<string> listaTratamientos{ get; set; } = new List<string>();
        public string mensaje_error = ""; // Variable para almacenar mensajes de error
        public string mensaje_exito = ""; // Variable para almacenar mensajes de éxito

        /// <summary>
        /// Método que se ejecuta cuando se despliega los combobox para los ID de citas y tratamientos.
        /// Objetivo: Extraer los datos de la base.
        /// Entradas: None
        /// Salidas: ID de citas y tratamientos
        /// </summary>
        public void OnGet()
        {
            conexion.abrir();
            string script_cita = "SELECT ID_cita FROM Cita";
            MySqlCommand command_paciente = conexion.obtenerComando(script_cita);

            using (MySqlDataReader reader = command_paciente.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ID = "" + reader.GetInt32(0);
                    listaCitas.Add(ID);
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
        /// Objetivo: Recibir los datos del formulario de cita - tratamiento, insertarlos en la base de datos y manejar errores.
        /// Entradas: Datos del formulario (ID_cita, ID_tratamiento).
        /// Salidas: Mensaje de éxito o mensaje de error.
        /// Restricciones: Todos los campos deben estar debidamente validados antes de enviarse.
        /// </summary>
        public void OnPost() 
        {
            Cita_tratamiento.ID_cita = Request.Form["id_cita"];
            Cita_tratamiento.ID_tratamiento = Request.Form["id_tratamiento"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Cita_tratamientos (ID_cita, ID_tratamiento)
                    VALUES (@ID_cita, @ID_tratamiento)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_cita", Cita_tratamiento.ID_cita);
                command.Parameters.AddWithValue("@ID_tratamiento", Cita_tratamiento.ID_tratamiento);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Cita_tratamiento.ID_cita = "";
                Cita_tratamiento.ID_tratamiento = "";

                mensaje_exito = "Formulario registrado exitosamente";
                conexion.cerrar();
            }
            catch (Exception ex)
            {
                mensaje_error = ex.Message;
                conexion.cerrar();
            }
        }


        // Clase que representa el modelo de los datos de Cita_tratamiento
        public class Cita_tratamientoInfo
        {
            public string ID_cita { get; set; }
            public string ID_tratamiento { get; set; }
        }
    }
}
