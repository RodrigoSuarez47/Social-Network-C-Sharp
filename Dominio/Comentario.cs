using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Comentario : Publicacion
    {
        public Comentario()
        {
        }
        public Comentario(string Ptexto, DateTime Pfecha, string Ptitulo, Miembro unMiembro) : base(Ptexto, Pfecha, Ptitulo, unMiembro)
        {
            Texto = Ptexto;
            FechaPublicacion = Pfecha;
            Titulo = Ptitulo;
            Autor = unMiembro;
            ValorDeAceptacion = CalcularAceptacion();
        }


        public override string ToString()
        {
            return $"\nPublicacion: de tipo Comentario numero {Id}\nTitulo: {Titulo}\nNombre: {Autor.Nombre} {Autor.Apellido}";
        }
        public override decimal CalcularAceptacion()
        {
            return CalcularVA(CalcularLikes(), CalcularDisLikes());
        }
    }
}
