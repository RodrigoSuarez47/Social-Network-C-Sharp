using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Publicacion: IValidable, IAutomaticId, IComparable<Publicacion>
    {
        //Atributos:
        public int Id { get; set; }
        public static int _ultId = 1;
        public string Texto { get; set; }   
        public DateTime FechaPublicacion { get; set; }
        public Miembro Autor { get; set; }
        public string Titulo { get; set; }
        public decimal ValorDeAceptacion { get; set; }
        public List<Reaccion> Reacciones { get; set; } = new List<Reaccion>();

        //Constructor:

        public Publicacion()
        {

        }
        public Publicacion(string Ptexto, DateTime Pfecha, string Ptitulo, Miembro unMiembro ) 
        {
            Id = _ultId++;
            Texto = Ptexto;
            FechaPublicacion = Pfecha;
            Titulo = Ptitulo;
            Autor = unMiembro;
            ValorDeAceptacion = CalcularAceptacion();
            Validar();
        }

        //Funcionalidades:

        public void Validar() 
        {
            Utilidades.ComprobarTextoVaio(Texto);
            Utilidades.ComprobarTextoVaio(Titulo);
            Utilidades.ObjetoNulo(Autor);
        }

        public virtual void AltaNuevaReaccion(Reaccion Preaccion)
        {
            bool reacciono = ComprobarReaccion(Preaccion.Miembro);
            if(!reacciono)
            {
                Reacciones.Add(Preaccion);
            }
            if(reacciono)
            {
                foreach(Reaccion reaccion in Reacciones)
                {
                    if(reaccion.Miembro.Email== Preaccion.Miembro.Email && reaccion.Accion== Preaccion.Accion)
                    {
                        Reacciones.Remove(reaccion);
                    }
                    else if (reaccion.Miembro.Email == Preaccion.Miembro.Email && reaccion.Accion != Preaccion.Accion)
                    {
                        Reacciones.Remove(reaccion);
                        Reacciones.Add(Preaccion);
                    }
                }
            }
        }
        public int CompareTo(Publicacion obj)
        {
            if (obj == null) return -1; 
            return FechaPublicacion.CompareTo(obj.FechaPublicacion)*-1;
        }
        public abstract decimal CalcularAceptacion();
        public decimal CalcularVA(int reaccionLike, int reaccionDislike)
        {
            return reaccionLike * 5 + reaccionDislike * (-2);
        }
        public int CalcularLikes()
        {
            int ret = 0;
            foreach(Reaccion reaccion in Reacciones)
            {
                if (reaccion.Accion.ToLower()== "like")ret++;
            }
            return ret;
        }
        public int CalcularDisLikes()
        {
            int ret = 0;
            foreach (Reaccion reaccion in Reacciones)
            {
                if (reaccion.Accion.ToLower() == "dislike") ret++;
            }
            return ret;
        }
        private bool ComprobarReaccion(Miembro miembro)
        {
            foreach(Reaccion reaccion in Reacciones)
            {
                if(miembro== reaccion.Miembro)return true;
            }
            return false;
        }
        public virtual bool DioLike(Miembro miembro)
        {
            foreach(Reaccion reaccion in Reacciones)
            {
                if(reaccion.Miembro.Email==  miembro.Email && reaccion.Accion.ToLower()== "like")return true; 
            }
            return false;
        }
        public virtual bool DiodisLike(Miembro miembro)
        {
            foreach (Reaccion reaccion in Reacciones)
            {
                if (reaccion.Miembro.Email == miembro.Email && reaccion.Accion.ToLower() == "dislike") return true;
            }
            return false;
        }
    }
}
