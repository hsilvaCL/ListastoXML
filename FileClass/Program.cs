using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Serialization;
using lib;


namespace FileClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Persona oPersona = new Persona();

            List<Persona> lPersonas= oPersona.GenerarListado();

            if (!DBLocal.VerificaConexion())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Offline");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Online");
            }
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Persona oPer in lPersonas) {
                Console.WriteLine(oPer.ToString());
            }
            Console.ReadLine();
        }
    }
}
