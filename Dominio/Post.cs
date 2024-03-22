using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Post : Publicacion, IValidable
    {
        //Atributos
        public string Imagen { get; set; }
        public bool Censurado { get; set; }
        public bool Privado { get; set; }
        private List<Comentario> _comentarios = new List<Comentario>();
        public List<Comentario> Comentarios { get { return _comentarios; } }
        //Constructor
        public Post() { }
        public Post(string Pimagen, bool Pcensurado, bool Pprivado, string Ptexto, DateTime Pfecha, string Ptitulo, Miembro unMiembro) : base(Ptexto, Pfecha, Ptitulo, unMiembro)
        {
            Imagen = Pimagen;
            Censurado = Pcensurado;
            Privado = Pprivado;
            Validar();
        }
        public override string ToString()
        {
            return $"\nPublicacion: de tipo Post numero {Id}\nTitulo: {Titulo}\nNombre: {Autor.Nombre} {Autor.Apellido}";
        }
        public void AltaNuevoComentario(Comentario Comentario)
        {
            _comentarios.Add(Comentario);
        }

        public bool RetornarPostComentado(string email)
        {
            foreach (Comentario comentario in _comentarios)
            {
                if (comentario.Autor.Email == email) return true;
            }
            return false;
        }

        public List<Comentario> RetornarComentarios(string email)
        {
            List<Comentario> comentarios = new List<Comentario>();
            foreach (Comentario comentario in _comentarios)
            {
                if (comentario.Autor.Email == email) comentarios.Add(comentario);
            }
            return comentarios;
        }
        public List<Comentario> RetornarMisComentarios()
        {
            return _comentarios;
        }
        public override decimal CalcularAceptacion()
        {
            decimal ret = 0;
            decimal VA = CalcularVA(CalcularLikes(), CalcularDisLikes());
            ret = VA;
            if (!Privado) ret += 10;
            return ret;
        }
        public Comentario ObtenerComentarioPost(int idComentario)
        {
            foreach (Comentario comentario in _comentarios)
            {
                if (comentario.Id == idComentario)
                    return comentario;
            }
            return null;
        }
        public bool ComentarioEnPost(Comentario comentario)
        {
            if (ObtenerComentarioPost(comentario.Id) != null) return true;
            return false;
        }
        public List<Publicacion> ObtenerComentarioPostPorAceptacion(string texto, string email, int aceptacion)
        {
            List<Publicacion> aux = new List<Publicacion>();
            foreach (Comentario comentario in _comentarios)
            {
                if (comentario.Texto.ToLower().Contains(texto.ToLower()) &&
                    comentario.CalcularAceptacion() > aceptacion)
                {
                    aux.Add(comentario);
                }
            }
            return aux;
        }
    }
}
