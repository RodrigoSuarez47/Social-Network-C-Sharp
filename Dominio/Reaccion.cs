using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dominio
{
    public class Reaccion: IValidable
    {
        //Atributos:
        public string Accion { get; set; }
        public Miembro Miembro { get; set; }

        //Constructor 
        public Reaccion (string Paccion, Miembro Pmiembro) 
        {
            Accion = Paccion;
            Miembro = Pmiembro;
            Validar();
        }

        public void Validar()
        {
            Utilidades.ComprobarTextoVaio(Accion);
            Utilidades.ObjetoNulo(Miembro);
            if (Accion.ToLower() != "like" && Accion.ToLower() != "dislike")
            {
                throw new Exception("Reaccion no valida");
            }
        }
    }
}
