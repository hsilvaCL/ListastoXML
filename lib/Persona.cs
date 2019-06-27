using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lib
{


    public class Persona : DBLocal
    {
        private string _nombre;
        private string _apellido;
        private string _direccion;

        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Apellido { get => _apellido; set => _apellido = value; }
        public string Direccion { get => _direccion; set => _direccion = value; }

        public List<Persona> GenerarListado()
        {
            Persona oPer = new Persona();
            List<Persona> lPersonas = new List<Persona>();

            if (!VerificaConexion())
            {
                lPersonas = RecuperaXML(this.GetType()).Cast<Persona>().ToList();
            }
            else
            {
                lPersonas.Add(new Persona() { Nombre = "Uno", Apellido = "Uno", Direccion = "Uno" });
                lPersonas.Add(new Persona() { Nombre = "Dos", Apellido = "Dos", Direccion = "Dos" });
                lPersonas.Add(new Persona() { Nombre = "Tres", Apellido = "Tres", Direccion = "Tres" });
            }
            oPer = new Persona() { Nombre = "Uno", Apellido = "Uno", Direccion = "Uno" };
            GeneraXMLRegistro(oPer);
            GeneraXML(lPersonas);
            return lPersonas;
        }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            st.AppendFormat("Nombre:{0} Apellido:{1}", Nombre, Apellido);
            return st.ToString();
        }
    }
}
