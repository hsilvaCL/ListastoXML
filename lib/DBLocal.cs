using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lib
{
    public class DBLocal
    {
        /*
         * Uso variable de sistema y declaración de directorio temporal
         * https://docs.microsoft.com/en-us/dotnet/api/system.environment.expandenvironmentvariables?view=netframework-4.8
         */
        public static string sruta = Environment.ExpandEnvironmentVariables("%AppData%\\FileClass");
        public static string sdbname = "OnBreak";
        public static string shost = "localhost";

        public DBLocal()
        {
            RevisaDirectorio();
        }

        /*Creación de XML de respaldo*/
        public void GeneraXML(object olistado)
        {
            string snomclase = olistado.GetType().GenericTypeArguments[0].Name;
            XmlSerializer xmlFile = new System.Xml.Serialization.XmlSerializer(olistado.GetType());

            using (FileStream fs = new FileStream(@sruta + "\\" + snomclase + ".xml", FileMode.Create))
            {
                xmlFile.Serialize(fs, olistado);
                fs.Close();
            }

        }

        /*Recuperación de XML y creación de lista en función de la clase de negocio*/
        public List<object> RecuperaXML(Type tipo)
        {
            /*Uso de tipo para instanciar una lista según la clase que corresponda*/
            IList lst = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(tipo)));

            XmlSerializer xmlFile = new System.Xml.Serialization.XmlSerializer(lst.GetType());
            FileStream fs = new FileStream(@sruta + "\\" + tipo.Name + ".xml", FileMode.Open);

            lst = (IList)xmlFile.Deserialize(fs);
            fs.Close();
            return lst.Cast<object>().ToList();

        }

        /*Valida la existencia del directorio que verifica los temporales
         Se usa static para realizar una llamada directa
             */
        public static bool VerificaConexion()
        {

            /*https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectiontimeout?view=netframework-4.8
                https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection?view=netframework-4.8
                */
            SqlConnection oConn = new SqlConnection("Data Source=" + shost + ";Initial Catalog=" + sdbname + ";Integrated Security=True;Connection Timeout=3");
            RevisaDirectorio();

            try
            {
                oConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(@sruta + "\\accesoDB.log"))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ":" + ex.Message);
                }

                return false;
            }
        }

        /*Valida la existencia del directorio que verifica los temporales
         https://www.dotnetperls.com/path
             */
        public static void RevisaDirectorio()
        {

            if (!Directory.Exists(sruta)) Directory.CreateDirectory(sruta);
        }

        /*Creación de XML de respaldo*/
        public void GeneraXMLRegistro(object oRegistro)
        {
            string snomclase = oRegistro.GetType().Name;
            XmlSerializer xmlFile = new System.Xml.Serialization.XmlSerializer(oRegistro.GetType());

            int i = RevisaArchivo(snomclase);

            using (FileStream fs = new FileStream(@sruta + "\\" + snomclase + i.ToString() + ".xml", FileMode.Create))
            {
                xmlFile.Serialize(fs, oRegistro);
                fs.Close();
            }

        }

        public int RevisaArchivo(string slnom)
        {
            int i = 0;

            do
            {
                i++;
            } while (File.Exists(sruta + "\\" + slnom + i.ToString() + ".xml"));

            return i;

        }

    }
}
