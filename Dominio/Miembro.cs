using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dominio
{
    public class Miembro:Usuario, IValidable, IComparable<Miembro>
    {
        //Atributos:
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        private List<Miembro> _amigos = new List<Miembro>();
        public List<Miembro> Amigos { get; }
        public bool Bloqueado { get; set; }
        //Constructor:
        public Miembro()
        {

        }
        public Miembro(string Pemail, string Pcontraseña, string Pnombre, string Papellido, DateTime PfechaNacimiento, bool Pbloqueado ):base(Pemail, Pcontraseña)
        {
            Email = Pemail;
            Contraseña = Pcontraseña;
            Nombre = Pnombre;
            Apellido = Papellido;
            FechaNacimiento = PfechaNacimiento;
            Bloqueado = Pbloqueado;
            Validar();
        }
        public void Validar()
        {
            base.Validar();
            Utilidades.ComprobarTextoVaio(Nombre);
            ValidarNombre(Nombre);
            Utilidades.ComprobarTextoVaio(Nombre);
            ValidarApellido(Apellido);
        }
        public static void ValidarApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
            {
                throw new Exception("El apellido no debe estar vacio");
            }
        }
        public static void ValidarNombre(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
            {
                throw new Exception("El apellido no debe estar vacio");
            }
        }
        public override string ToString()
        {
            return $"{Nombre} {Apellido}"; 
        }
        public void AgregarAmigo(Miembro NuevoAmigo)
        {
            _amigos.Add(NuevoAmigo);    
        }
        public bool ValidarAmistadPreEstablecida(Miembro miembro)
        {
            foreach(Miembro amigo in _amigos)
            {
                if (amigo.Email == miembro.Email) return false; 
            }
            return true;
        }
        public int CompareTo(Miembro miembro)
        {
            if (miembro == null)
            {
                return -1;
            }

            return Email.CompareTo(miembro.Email);
        }
        public List<Miembro> ObtenerListaAmigos()
        {
            return _amigos;
        }
    }
}
