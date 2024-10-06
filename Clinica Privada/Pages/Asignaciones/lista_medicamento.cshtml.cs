using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static Clinica_Privada.Pages.Asignaciones.cita_tratamientoModel;
using static Clinica_Privada.Pages.Asignaciones.lista_medicamentoModel;

namespace Clinica_Privada.Pages.Asignaciones
{
    public class lista_medicamentoModel : PageModel
    {
        public Tratamiento_MedicamentosInfo Tratamiento_Medicamentos { get; set; } = new Tratamiento_MedicamentosInfo(); // Lista que guarda toda la información de los requests
        public ConexionBD conexion = new ConexionBD(); // Instancia de la clase Conexion para manejar la conexión a la base de datos
        public List<string> listaTratamientos { get; set; } = new List<string>();
        public List<string> listaMedicamentos { get; set; } = new List<string>();
        public string mensaje_error = ""; // Variable para almacenar mensajes de error
        public string mensaje_exito = ""; // Variable para almacenar mensajes de éxito

        /// <summary>
        /// Método que se ejecuta cuando se despliega los combobox para el ID de los tratamientos y el nombre de los medicamentos.
        /// Objetivo: Extraer los datos de la base.
        /// Entradas: None
        /// Salidas: ID de tratamientos y nombre de los medicamentos
        /// </summary>
        public void OnGet()
        {
            conexion.abrir();
            string script_tratamiento = "SELECT ID_tratamiento FROM Tratamiento";
            MySqlCommand command_paciente = conexion.obtenerComando(script_tratamiento);

            using (MySqlDataReader reader = command_paciente.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ID = "" + reader.GetInt32(0);
                    listaTratamientos.Add(ID);
                }
            }
            conexion.cerrar();

            conexion.abrir();
            string script_medicamento = "SELECT nombre FROM Medicamento";
            MySqlCommand command_medico = conexion.obtenerComando(script_medicamento);

            using (MySqlDataReader reader = command_medico.ExecuteReader())
            {
                while (reader.Read())
                {
                    string nombre = "" + reader.GetInt32(0);
                    listaTratamientos.Add(nombre);
                }
            }
            conexion.cerrar();
        }

        /// <summary>
        /// Método que se ejecuta cuando se envía el formulario (POST request).
        /// Objetivo: Recibir los datos del formulario de lista_medicamento, insertarlos en la base de datos y manejar errores.
        /// Entradas: Datos del formulario (ID_tratamiento, nombre_medicamento).
        /// Salidas: Mensaje de éxito o mensaje de error.
        /// Restricciones: Todos los campos deben estar debidamente validados antes de enviarse.
        /// </summary>
        public void onPost() 
        {
            Tratamiento_Medicamentos.ID_tratamiento = Request.Form["id_cita"];
            Tratamiento_Medicamentos.nombre_medicamento = Request.Form["nombre_medicamento"];

            try
            {
                conexion.abrir();
                string query = @"
                    INSERT INTO Lista_medicamentos (ID_tratamiento, nombre_medicamento)
                    VALUES (@ID_tratamiento, @nombre_medicamento)";
                MySqlCommand command = conexion.obtenerComando(query);
                command.Parameters.AddWithValue("@ID_tratamiento", Tratamiento_Medicamentos.ID_tratamiento);
                command.Parameters.AddWithValue("@nombre_medicamento", Tratamiento_Medicamentos.nombre_medicamento);

                command.ExecuteNonQuery();


                // Limpieza del formulario
                Tratamiento_Medicamentos.ID_tratamiento = "";
                Tratamiento_Medicamentos.nombre_medicamento = "";

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

        public class Tratamiento_MedicamentosInfo
        {
            public string ID_tratamiento { get; set; }
            public string nombre_medicamento { get; set; }
        }
    }
}
