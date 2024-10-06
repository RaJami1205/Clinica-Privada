using MySql.Data.MySqlClient;
using System;

namespace Clinica_Privada.Pages
{
    public class ConexionBD
    {
        public MySqlConnection ConectarBD = new MySqlConnection("server=localhost;user id=root;database=clinica_privada;password=Albatroz1205$");

        // Método para abrir la conexión
        public void abrir()
        {
            try
            {
                ConectarBD.Open();
                Console.WriteLine("Conexión abierta");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: No se pudo abrir la BD " + ex.Message);
            }
        }

        // Método para cerrar la conexión
        public void cerrar()
        {
            try
            {
                if (ConectarBD.State == System.Data.ConnectionState.Open)
                {
                    ConectarBD.Close();
                    Console.WriteLine("Conexión cerrada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: No se pudo cerrar la BD " + ex.Message);
            }
        }

        // Método para obtener un comando MySQL
        public MySqlCommand obtenerComando(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, ConectarBD);
            return command;
        }
    }
}