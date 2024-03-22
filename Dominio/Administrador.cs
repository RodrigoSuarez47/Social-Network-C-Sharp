using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Administrador : Usuario, IValidable
    {
        ////Constructor
        
        public Administrador()
        { 
        }

        public Administrador(string Pemail, string Pcontraseña) : base(Pemail, Pcontraseña)
        {
            Email = Pemail;
            Contraseña = Pcontraseña;

        }
        //Validaciones
        public void Validar()
        {
            Utilidades.ComprobarTextoVaio(Email);
            Utilidades.ValidarFormatoEmail(Email);
            Utilidades.ComprobarTextoVaio(Contraseña);
            Utilidades.ValidarContraseña(Contraseña);
        }
        //Funcionalidades:
        public override string ToString()
        {
            return $"Email: {Email} \nContrasena: {Contraseña}";
        }

    }

}
