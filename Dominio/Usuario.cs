using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Usuario: IValidable, IEquatable<Miembro>
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }
        //Constructor:

        public Usuario()
        {

        }

        public Usuario(string Pemail, string Pcontraseña)
        {
            Email = Pemail;
            Contraseña = Pcontraseña;
            Validar();
        }

        public void Validar()
        {
            Utilidades.ComprobarTextoVaio(Email);
            Utilidades.ValidarFormatoEmail(Email);
            Utilidades.ComprobarTextoVaio(Contraseña);
            Utilidades.ValidarContraseña(Contraseña);
        }
        public bool Equals(Miembro miembro)
        {
            return miembro != null && miembro.Email==Email;
        }


    }
}
