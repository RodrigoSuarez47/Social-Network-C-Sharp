using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Dominio.Solicitud;

namespace Dominio
{
    public class Sistema
    {
        private static Sistema _instancia;
        private List<Miembro> _miembros = new List<Miembro>();
        private List<Administrador> _administradores = new List<Administrador>();
        private List<Solicitud> _solicitudes = new List<Solicitud>();
        private List<Post> _posts = new List<Post>();

        public List<Miembro> Miembros { get { return _miembros; } }
        public List<Solicitud> Solicitudes { get { return _solicitudes; } }
        public List<Administrador> Administradores { get { return _administradores; } }
        public List<Post> Publicaciones { get { return _posts; } }

        public static Sistema Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new Sistema();
                return _instancia;
            }
        }

        private Sistema()
        {
            PrecargarDatos();
        }

        public void PrecargarDatos()
        {
            PrecargarAdmin();
            PrecargarMiembros();
            PrecargarPublicaciones();
            PrecargarAmistades();
            PrecargarSolicitudesAmistad();
        }

        public void PrecargarAdmin()
        {
            Administrador unAdmin = new Administrador("admin1@algo.com", "Password1");
            CargarAdministrador(unAdmin);
            Administrador unAdmin2 = new Administrador("admin2@algo.com", "Password2");
            CargarAdministrador(unAdmin2);
        }

        public void ComprobarEmailRepetidoAdmin(string email)
        {
            foreach (Administrador admin in _administradores)
            {
                if (admin.Email == email)
                {
                    throw new Exception("El email ingresado ya fue registrado por otro usuario");
                }
            }
        }

        public void CargarAdministrador(Administrador admin)
        {
            if (admin == null)
            {
                throw new Exception("El administrador no puede ser un nulo.");
            }
            ComprobarEmailRepetidoAdmin(admin.Email);
            admin.Validar();
            _administradores.Add(admin);
        }

        private void PrecargarSolicitudesAmistad()
        {
            Solicitud Solicitud = new Solicitud(_miembros[5], _miembros[8], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud);
            Solicitud Solicitud2 = new Solicitud(_miembros[2], _miembros[4], Estado.Aprobado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud2);
            Solicitud Solicitud3 = new Solicitud(_miembros[10], _miembros[3], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud3);
            Solicitud Solicitud4 = new Solicitud(_miembros[3], _miembros[4], Estado.Rechazado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud4);
            Solicitud Solicitud5 = new Solicitud(_miembros[4], _miembros[6], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud5);
            Solicitud Solicitud6 = new Solicitud(_miembros[7], _miembros[8], Estado.Aprobado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud6);
            Solicitud Solicitud7 = new Solicitud(_miembros[2], _miembros[3], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud7);
            Solicitud Solicitud8 = new Solicitud(_miembros[8], _miembros[6], Estado.Rechazado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud8);
            Solicitud Solicitud9 = new Solicitud(_miembros[2], _miembros[9], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud9);
            Solicitud Solicitud10 = new Solicitud(_miembros[10], _miembros[5], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud10);
            Solicitud Solicitud11 = new Solicitud(_miembros[2], _miembros[8], Estado.Rechazado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud11);
            Solicitud Solicitud12 = new Solicitud(_miembros[8], _miembros[3], Estado.Aprobado, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud12);
            Solicitud Solicitud13 = new Solicitud(_miembros[11], _miembros[3], Estado.Pendiente, new DateTime(2023, 09, 16));
            AltaNuevaSolicitud(Solicitud13);
        }

        public void AltaNuevaSolicitud(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new Exception("La solicitud es un objeto nulo");
            }
            switch (solicitud.EstadoSolicitud)
            {
                case Estado.Pendiente:
                case Estado.Aprobado:
                case Estado.Rechazado:
                    solicitud.Validar();
                    if (solicitud.EstadoSolicitud == Estado.Aprobado)
                    {
                        AltaNuevaAmistad(solicitud.Solicitante, solicitud.Solicitado);
                    }
                    _solicitudes.Add(solicitud);
                    break;

                default:
                    throw new Exception("El estado es inválido");
            }
        }

        private void PrecargarAmistades()
        {
            //Amistad entre unMiembro1 hasta el 5 
            AltaNuevaAmistad(_miembros[0], _miembros[1]);
            AltaNuevaAmistad(_miembros[0], _miembros[2]);
            AltaNuevaAmistad(_miembros[0], _miembros[3]);
            AltaNuevaAmistad(_miembros[0], _miembros[4]);
            AltaNuevaAmistad(_miembros[0], _miembros[5]);
            //Amistad entre unMiembro2 desde el 6 al 12
            AltaNuevaAmistad(_miembros[1], _miembros[6]);
            AltaNuevaAmistad(_miembros[1], _miembros[7]);
            AltaNuevaAmistad(_miembros[1], _miembros[8]);
            AltaNuevaAmistad(_miembros[1], _miembros[9]);
            AltaNuevaAmistad(_miembros[1], _miembros[10]);
            AltaNuevaAmistad(_miembros[1], _miembros[11]);
        }

        private void PrecargarMiembros()
        {
            Miembro unMiembro1 = new Miembro("miembro1@algo.com", "Password1", "Nombre1", "Apellido1", new DateTime(2022, 01, 01), false);
            CargarMiembro(unMiembro1);
            Miembro unMiembro2 = new Miembro("miembro2@algo.com", "Password2", "Nombre2", "Apellido2", new DateTime(2022, 02, 02), true);
            CargarMiembro(unMiembro2);
            Miembro unMiembro3 = new Miembro("miembro3@algo.com", "Password3", "Nombre3", "Apellido3", new DateTime(2022, 03, 03), false);
            CargarMiembro(unMiembro3);
            Miembro unMiembro4 = new Miembro("miembro4@algo.com", "Password4", "Nombre4", "Apellido4", new DateTime(2022, 04, 04), false);
            CargarMiembro(unMiembro4);
            Miembro unMiembro5 = new Miembro("miembro5@algo.com", "Password5", "Nombre5", "Apellido5", new DateTime(2022, 05, 05), false);
            CargarMiembro(unMiembro5);
            Miembro unMiembro6 = new Miembro("miembro6@algo.com", "Password6", "Nombre6", "Apellido6", new DateTime(2022, 06, 06), true);
            CargarMiembro(unMiembro6);
            Miembro unMiembro7 = new Miembro("miembro7@algo.com", "Password7", "Nombre7", "Apellido7", new DateTime(2022, 07, 07), false);
            CargarMiembro(unMiembro7);
            Miembro unMiembro8 = new Miembro("miembro8@algo.com", "Password8", "Nombre8", "Apellido8", new DateTime(2022, 08, 08), false);
            CargarMiembro(unMiembro8);
            Miembro unMiembro9 = new Miembro("miembro9@algo.com", "Password9", "Nombre9", "Apellido9", new DateTime(2022, 09, 09), false);
            CargarMiembro(unMiembro9);
            Miembro unMiembro10 = new Miembro("miembro10@algo.com", "Password10", "Nombre10", "Apellido10", new DateTime(2022, 10, 10), true);
            CargarMiembro(unMiembro10);
            Miembro unMiembro11 = new Miembro("miembro11@algo.com", "Password11", "Nombre11", "Apellido11", new DateTime(2022, 11, 11), false);
            CargarMiembro(unMiembro11);
            Miembro unMiembro12 = new Miembro("miembro12@algo.com", "Password12", "Nombre12", "Apellido12", new DateTime(2022, 12, 12), false);
            CargarMiembro(unMiembro12);
        }

        public void CargarMiembro(Miembro unMiembro)
        {
            if (unMiembro == null)
            {
                throw new Exception("El miembro no puede ser un nulo.");
            }
            unMiembro.Email = unMiembro.Email.ToLower();
            ComprobarEmailRepetidoMiembro(unMiembro.Email);
            unMiembro.Validar();
            _miembros.Add(unMiembro);
        }

        private void ComprobarEmailRepetidoMiembro(string email)
        {
            foreach (Miembro unMiembro in _miembros)
            {
                if (unMiembro.Email == email)
                {
                    throw new Exception("El email ya fue registrado para otro usuario");
                }
            }
        }

        public void PrecargarPublicaciones()
        {
            // Posts
            Post Post1 = new Post("Imagen1.jpg", false, false, "Texto del post 1", new DateTime(2022, 9, 1), "Titulo de post 1", _miembros[0]);
            ValidarPublicacion(Post1);
            Post Post2 = new Post("Imagen2.jpg", true, false, "Texto del post 2", new DateTime(2023, 9, 2), "Titulo de post 2", _miembros[0]);
            ValidarPublicacion(Post2);
            Post Post3 = new Post("Imagen3.jpg", false, false, "Texto del post 3", new DateTime(2023, 9, 3), "Titulo de post 3", _miembros[1]);
            ValidarPublicacion(Post3);
            Post Post4 = new Post("Imagen4.jpg", false, false, "Texto del post 4", new DateTime(2023, 9, 4), "Titulo de post 4", _miembros[1]);
            ValidarPublicacion(Post4);
            Post Post5 = new Post("Imagen5.jpg", false, false, "Texto del post 5", new DateTime(2023, 9, 5), "Titulo de post 5", _miembros[3]);
            ValidarPublicacion(Post5);
            //Reacciones a Post:
            Reaccion reaccion1 = new Reaccion("Like", _miembros[0]);
            Reaccion reaccion2 = new Reaccion("Dislike", _miembros[0]);
            Reaccion reaccion3 = new Reaccion("Like", _miembros[1]);
            Reaccion reaccion4 = new Reaccion("Dislike", _miembros[1]);
            Reaccion reaccion5 = new Reaccion("Like", _miembros[2]);
            Post1.AltaNuevaReaccion(reaccion1);
            Post2.AltaNuevaReaccion(reaccion2);
            Post1.AltaNuevaReaccion(reaccion3);
            Post2.AltaNuevaReaccion(reaccion4);
            Post1.AltaNuevaReaccion(reaccion5);

            // Comentarios
            Comentario Comentario1 = new Comentario("Texto de comentario 1", new DateTime(2023, 9, 1), "Titulo de comentario 1", _miembros[0]);
            Comentario Comentario2 = new Comentario("Texto de comentario 2", new DateTime(2023, 9, 2), "Titulo de comentario 2", _miembros[0]);
            Comentario Comentario3 = new Comentario("Texto de comentario 3", new DateTime(2023, 9, 3), "Titulo de comentario 3", _miembros[0]);
            Comentario Comentario4 = new Comentario("Texto de comentario 4", new DateTime(2023, 9, 4), "Titulo de comentario 4", _miembros[0]);
            Comentario Comentario5 = new Comentario("Texto de comentario 5", new DateTime(2023, 9, 5), "Titulo de comentario 5", _miembros[0]);
            //   
            Comentario Comentario6 = new Comentario("Texto de comentario 6", new DateTime(2023, 9, 6), "Titulo de comentario 6", _miembros[1]);
            Comentario Comentario7 = new Comentario("Texto de comentario 7", new DateTime(2023, 9, 7), "Titulo de comentario 7", _miembros[1]);
            Comentario Comentario8 = new Comentario("Texto de comentario 8", new DateTime(2023, 9, 8), "Titulo de comentario 8", _miembros[1]);
            Comentario Comentario9 = new Comentario("Texto de comentario 9", new DateTime(2023, 9, 9), "Titulo de comentario 9", _miembros[1]);
            Comentario Comentario10 = new Comentario("Texto de comentario 10", new DateTime(2023, 9, 10), "Titulo de comentario 10", _miembros[1]);
            //
            Comentario Comentario11 = new Comentario("Texto de comentario 11", new DateTime(2023, 9, 11), "Titulo de comentario 11", _miembros[2]);
            Comentario Comentario12 = new Comentario("Texto de comentario 12", new DateTime(2023, 9, 12), "Titulo de comentario 12", _miembros[2]);
            Comentario Comentario13 = new Comentario("Texto de comentario 13", new DateTime(2023, 9, 13), "Titulo de comentario 13", _miembros[2]);
            Comentario Comentario14 = new Comentario("Texto de comentario 14", new DateTime(2023, 9, 14), "Titulo de comentario 14", _miembros[2]);
            Comentario Comentario15 = new Comentario("Texto de comentario 15", new DateTime(2023, 9, 15), "Titulo de comentario 15", _miembros[2]);
            //
            Comentario Comentario16 = new Comentario("Texto de comentario 16", new DateTime(2023, 9, 16), "Titulo de comentario 16", _miembros[3]);
            Comentario Comentario17 = new Comentario("Texto de comentario 17", new DateTime(2023, 9, 17), "Titulo de comentario 17", _miembros[3]);
            Comentario Comentario18 = new Comentario("Texto de comentario 18", new DateTime(2023, 9, 18), "Titulo de comentario 18", _miembros[3]);
            Comentario Comentario19 = new Comentario("Texto de comentario 19", new DateTime(2023, 9, 19), "Titulo de comentario 19", _miembros[3]);
            Comentario Comentario20 = new Comentario("Texto de comentario 20", new DateTime(2023, 9, 20), "Titulo de comentario 20", _miembros[3]);
            //
            Comentario Comentario21 = new Comentario("Texto de comentario 21", new DateTime(2023, 9, 21), "Titulo de comentario 21", _miembros[4]);
            Comentario Comentario22 = new Comentario("Texto de comentario 22", new DateTime(2023, 9, 22), "Titulo de comentario 22", _miembros[4]);
            Comentario Comentario23 = new Comentario("Texto de comentario 23", new DateTime(2023, 9, 23), "Titulo de comentario 23", _miembros[4]);
            Comentario Comentario24 = new Comentario("Texto de comentario 24", new DateTime(2023, 9, 24), "Titulo de comentario 24", _miembros[4]);
            Comentario Comentario25 = new Comentario("Texto de comentario 25", new DateTime(2023, 9, 25), "Titulo de comentario 25", _miembros[4]);
            //Agregando comentarios a posts 
            Post1.AltaNuevoComentario(Comentario1);
            Post1.AltaNuevoComentario(Comentario2);
            Post1.AltaNuevoComentario(Comentario3);
            Post1.AltaNuevoComentario(Comentario4);
            Post1.AltaNuevoComentario(Comentario5);
            //
            Post2.AltaNuevoComentario(Comentario6);
            Post2.AltaNuevoComentario(Comentario7);
            Post2.AltaNuevoComentario(Comentario8);
            Post2.AltaNuevoComentario(Comentario9);
            Post2.AltaNuevoComentario(Comentario10);
            //
            Post3.AltaNuevoComentario(Comentario11);
            Post3.AltaNuevoComentario(Comentario12);
            Post3.AltaNuevoComentario(Comentario13);
            Post3.AltaNuevoComentario(Comentario14);
            Post3.AltaNuevoComentario(Comentario15);
            //
            Post4.AltaNuevoComentario(Comentario16);
            Post4.AltaNuevoComentario(Comentario17);
            Post4.AltaNuevoComentario(Comentario18);
            Post4.AltaNuevoComentario(Comentario19);
            Post4.AltaNuevoComentario(Comentario20);
            //
            Post5.AltaNuevoComentario(Comentario21);
            Post5.AltaNuevoComentario(Comentario22);
            Post5.AltaNuevoComentario(Comentario23);
            Post5.AltaNuevoComentario(Comentario24);
            Post5.AltaNuevoComentario(Comentario25);
            //Reacciones a Comentarios:
            Reaccion reaccion6 = new Reaccion("Like", _miembros[0]);
            Comentario1.AltaNuevaReaccion(reaccion6);
            Reaccion reaccion7 = new Reaccion("Dislike", _miembros[1]);
            Comentario2.AltaNuevaReaccion(reaccion7);
            Reaccion reaccion8 = new Reaccion("Like", _miembros[0]);
            Comentario3.AltaNuevaReaccion(reaccion8);
            Reaccion reaccion9 = new Reaccion("Dislike", _miembros[1]);
            Comentario4.AltaNuevaReaccion(reaccion9);
            Reaccion reaccion10 = new Reaccion("Like", _miembros[2]);
            Comentario5.AltaNuevaReaccion(reaccion10);
        }

        public void ValidarPublicacion(Post Ppublicacion)
        {
            if (Ppublicacion == null)
            {
                throw new Exception("La publicacion no puede ser nula.");
            }
            if (string.IsNullOrEmpty(Ppublicacion.Titulo) || (Ppublicacion.Titulo).Length < 3)
            {
                throw new Exception("El titulo no puede ser vacio y debe tener al menos 3 caracteres.");
            }
            if (Ppublicacion.Texto == string.Empty || Ppublicacion.Titulo == "")
            {
                throw new Exception("El contenido de la publicacion no puede ser vacio.");
            }
            _posts.Add(Ppublicacion);
        }

        public Miembro? BuscarMiembroPorMail(string Pemail)
        {
            Utilidades.ValidarFormatoEmail(Pemail);
            foreach (Miembro unMiembro in _miembros)
            {
                if (unMiembro.Email == Pemail)
                {
                    return unMiembro;
                }
            }
            return null;
        }

        public string ListarPublicacionesDeMiembro(Miembro Pmiembro)
        {
            if (Pmiembro == null)
            {
                throw new Exception("El miembro no puede ser nulo.");
            }
            Pmiembro.Validar();
            string resultado = "";
            List<Post> posts = ObtenerPosts(Pmiembro);
            resultado += ListarPosts(posts);
            resultado += "\n";
            List<Comentario> Comentarios = ObtenerComentarios(Pmiembro);
            resultado += ListarComentarios(Comentarios);

            if (posts.Count < 1 && Comentarios.Count < 1)
            {
                throw new Exception("No se han encontrado publicaciones del ususario indicado");
            }
            return resultado;
        }

        public void AltaNuevaAmistad(Miembro solicitante, Miembro solicitado)
        {
            if (solicitante == null || solicitado == null) throw new Exception("Uno de los dos miembros no existe");
            solicitante.Validar();
            solicitado.Validar();
            if (!solicitante.ValidarAmistadPreEstablecida(solicitado)) throw new Exception($"Ya hay una amistad entre usuario email :{solicitante.Email} y {solicitado.Email}");
            solicitado.AgregarAmigo(solicitante);
            solicitante.AgregarAmigo(solicitado);
        }

        private string ListarPosts(List<Post> posts)
        {
            string ret = string.Empty;
            foreach (Post post in posts)
            {
                ret += post;
            }
            return ret;
        }

        private string ListarComentarios(List<Comentario> comentarios)
        {
            string respuesta = string.Empty;
            foreach (Comentario comentario in comentarios)
            {
                respuesta += comentario;
            }
            return respuesta;
        }

        public List<Post> ObtenerPosts(Miembro miembro)
        {
            List<Post> ret = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.Autor.Email == miembro.Email) ret.Add(post);
            }
            return ret;
        }

        public List<Comentario> ObtenerComentarios(Miembro miembro)
        {
            List<Comentario> comentarios = new List<Comentario>();
            foreach (Post post in _posts)
            {
                comentarios.AddRange(post.RetornarComentarios(miembro.Email));
            }
            return comentarios;
        }

        public List<Post> ObtenerPostComentados(string email)
        {
            List<Post> posts = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.RetornarPostComentado(email)) posts.Add(post);
            }
            return posts;
        }

        public string ObtenerMiembrosConMasPublicaciones()
        {
            string ret = string.Empty;
            int masPublicaciones = 0;
            List<Miembro> miembros = new List<Miembro>();
            foreach (Miembro miembro in _miembros)
            {
                int numComentarios = ObtenerComentarios(miembro).Count;
                int numPost = ObtenerPosts(miembro).Count;
                int aux = numComentarios + numPost;
                if (aux == masPublicaciones) miembros.Add(miembro);
                if (aux > masPublicaciones)
                {
                    miembros.Clear();
                    miembros.Add(miembro);
                    masPublicaciones = aux;
                }
            }
            ret = ObtenerListaMiembros(miembros);
            return ret;
        }

        private string ObtenerListaMiembros(List<Miembro> miembros)
        {
            string ret = string.Empty;
            foreach (Miembro miembro in miembros)
            {
                ret += miembro + "\n";
            }
            return ret;
        }

        public List<Post> ObtenerPostPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Post> ret = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.FechaPublicacion >= fechaInicio && post.FechaPublicacion <= fechaFin) ret.Add(post);
            }
            return ret;
        }

        public string MostrarPostsPorFecha(List<Post> posts)
        {
            List<Post> postOrdenados = posts.OrderByDescending(post => post.Titulo).ToList();
            string retorno = string.Empty;
            foreach (Post post in postOrdenados)
            {
                retorno += $"Id: {post.Id} \n";
                retorno += $"Fecha: {post.FechaPublicacion} \n";
                retorno += $"Titulo: {post.Titulo} \n";
                if (post.Texto.Length <= 50)
                {
                    retorno += $"Texto: {post.Texto} \n";
                }
                else
                {
                    retorno += $"Texto: {post.Texto.Substring(0, 50)}...\n";
                }
                retorno += "\n";
            }
            return retorno;
        }

        public List<Post> ObtenerMisPublicaciones(string email)
        {
            List<Post> aux = new List<Post>();
            Miembro miembro = BuscarMiembroPorMail(email);
            if (miembro == null) throw new Exception("El email buscado no existe.");
            aux = ObtenerMisPublicacionesYpublicacionesAmigos(miembro);
            if (aux.Count == 0) throw new Exception("No se encontraron posts");
            return aux;
        }
        public List<Post> ObtenerMisPublicacionesYpublicacionesAmigos(Miembro miembro)
        {
            List<Post> aux = new List<Post>();
            aux = ObtenerPostsSinCensura(miembro);
            aux.AddRange(ObtenerPostsDeMisAmigos(miembro));
            aux.AddRange(ObtenerMisPostsPrivados(miembro));
            aux.Sort();
            return aux;
        }
        private List<Post> ObtenerMisPostsPrivados(Miembro miembro)
        {
            List<Post> aux = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post != null && post.Autor.Email == miembro.Email && post.Privado) aux.Add(post);
            }
            return aux;
        }
        public List<Post> ObtenerPostsDeMisAmigos(Miembro miembro)
        {
            List<Post> aux = new List<Post>();
            List<Miembro> amigos = miembro.ObtenerListaAmigos();
            foreach (Miembro amigo in amigos)
            {
                aux.AddRange(ObtenerPostsSinCensuraPublicos(amigo));
            }
            return aux;
        }
        public List<Post> ObtenerPostsSinCensura(Miembro miembro)
        {
            List<Post> ret = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.Autor.Email == miembro.Email && !post.Censurado && !post.Privado) ret.Add(post);
            }
            return ret;
        }
        public List<Post> ObtenerPostsSinCensuraPublicos(Miembro miembro)
        {
            List<Post> ret = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.Autor.Email == miembro.Email && !post.Censurado && !post.Privado) ret.Add(post);
            }
            return ret;
        }
        public List<Comentario> ObtenerComentariosDePostId(int id)
        {
            List<Comentario> aux = new List<Comentario>();
            Post post = ObtenerPostXId(id);
            if (post == null) throw new Exception("El post buscado no se encontro");
            aux.AddRange(post.RetornarMisComentarios());
            aux.Sort();
            return aux;
        }
        public Post ObtenerPostXId(int id)
        {
            foreach (Post post in _posts)
            {
                if (post.Id == id) return post;
            }
            return null;
        }
        public void AnadirReaccion(int postId, string email, string accion)
        {
            Post post = ObtenerPostXId(postId);
            if (post == null) throw new Exception("El post buscado no existe");
            Miembro miembro = BuscarMiembroPorMail(email);
            if (miembro == null) throw new Exception("El miembro buscado no existe");
            Reaccion reaccion = new Reaccion(accion, miembro);
            post.AltaNuevaReaccion(reaccion);
        }
        public List<Miembro> ObtenerListaLikesPost(int postId)
        {
            List<Miembro> aux = new List<Miembro>();
            Post post = ObtenerPostXId(postId);
            foreach (Miembro miembro in _miembros)
            {
                if (post.DioLike(miembro)) aux.Add(miembro);
            }
            return aux;
        }
        public List<Miembro> ObtenerListaDisLikesPost(int postId)
        {
            List<Miembro> aux = new List<Miembro>();
            Post post = ObtenerPostXId(postId);
            foreach (Miembro miembro in _miembros)
            {
                if (post.DiodisLike(miembro)) aux.Add(miembro);
            }
            return aux;
        }

        public void AñadirComentario(int PostId, Comentario comentario)
        {
            Post post = ObtenerPostXId(PostId);
            if (post == null) throw new Exception("El post buscado no existe");
            if (comentario == null) throw new Exception("El comentario no puede ser nulo");
            post.AltaNuevoComentario(comentario);
        }
        public Comentario ObtenerComentarioId(int idComentario)
        {
            Comentario comentario = new Comentario();
            foreach (Post post in _posts)
            {
                if (post.ObtenerComentarioPost(idComentario) != null)
                {
                    comentario = post.ObtenerComentarioPost(idComentario);
                    return comentario;
                }
            }
            if (comentario == null) throw new Exception("El comentario buscado no existe");
            return null;
        }
        public void AnadirReaccionComentario(int comentarioId, string email, string accion)
        {
            Miembro miembro = BuscarMiembroPorMail(email);
            if (miembro == null) throw new Exception("El miembro buscado no existe");
            Comentario comentario = ObtenerComentarioEnPosts(comentarioId);
            if (comentario == null) throw new Exception("El comentario buscado no existe");
            comentario.AltaNuevaReaccion(new Reaccion(accion, miembro));
        }
        public Comentario ObtenerComentarioEnPosts(int idComentario)
        {
            foreach (Post post in _posts)
            {
                Comentario comentario = post.ObtenerComentarioPost(idComentario);
                if (comentario != null && comentario.Id == idComentario)
                {
                    return comentario;
                }
            }
            return null;
        }
        public Post ObtenerPostPorComentario(Comentario comentario)
        {
            if (comentario == null) throw new Exception("El comentario buscado no existe;");
            foreach (Post post in _posts)
            {
                if (post.ComentarioEnPost(comentario)) return post;
            }
            return null;
        }
        public List<Miembro> ObtenerLikesComentario(int idComentario)
        {
            Comentario comentario = ObtenerComentarioEnPosts(idComentario);
            List<Miembro> aux = new List<Miembro>();
            foreach (Miembro miembro in _miembros)
            {
                if (comentario.DioLike(miembro)) aux.Add(miembro);
            }
            return aux;
        }
        public List<Miembro> ObtenerdisLikesComentario(int idComentario)
        {
            Comentario comentario = ObtenerComentarioId(idComentario);
            List<Miembro> aux = new List<Miembro>();
            foreach (Miembro miembro in _miembros)
            {
                if (comentario.DiodisLike(miembro)) aux.Add(miembro);
            }
            return aux;
        }

        public Administrador BuscarAdministradorPorEmail(string email)
        {
            foreach (Administrador administrador in _administradores)
            {
                if (administrador.Email == email) return administrador;
            }
            return null;
        }
        public Usuario BuscarUsuarioPorEmail(string email)
        {
            Usuario usuario = BuscarMiembroPorMail(email);
            if (usuario == null) usuario = BuscarAdministradorPorEmail(email);
            if (usuario == null) throw new Exception("El email ingresado no se encontro");
            return usuario;
        }
        public List<Post> ObtenerPublicacionesAdministrador()
        {
            List<Post> list = _posts;
            list.Sort();
            return list;
        }

        public void EnviarSolicitudAmistad(Miembro solicitante, Miembro solicitado)
        {
            if (solicitante == null || solicitado == null)
            {
                throw new Exception("Uno de los dos miembros no existe");
            }

            solicitante.Validar();
            solicitado.Validar();

            if (!solicitante.ValidarAmistadPreEstablecida(solicitado))
            {
                throw new Exception($"Ya hay una amistad entre usuario email: {solicitante.Email} y {solicitado.Email}");
            }

            if (ValidarSolicitudDeAmistad(solicitante, solicitado))
            {
                throw new Exception($"Ya hay una solicitud enviada de usuario email: {solicitante.Email} a {solicitado.Email}");
            }

            Solicitud nuevaSolicitud = new Solicitud(solicitante, solicitado, Solicitud.Estado.Pendiente, DateTime.Now);
            AltaNuevaSolicitud(nuevaSolicitud);
        }

        public bool ValidarSolicitudDeAmistad(Miembro solicitante, Miembro solicitado)
        {
            foreach (Solicitud solicitud in _solicitudes)
            {
                if ((solicitud.Solicitante.Email == solicitante.Email && solicitud.Solicitado.Email == solicitado.Email) ||
                    (solicitud.Solicitante.Email == solicitado.Email && solicitud.Solicitado.Email == solicitante.Email))
                {
                    return true;
                }
            }
            return false;
        }
        public List<Miembro> ListarMiembrosOrdenados()
        {
            List<Miembro> aux = _miembros;
            aux.Sort();
            return aux;
        }
        public List<Miembro> BuscarMiembroConEmail(string email)
        {
            List<Miembro> aux = new List<Miembro>();
            foreach (Miembro miembro in _miembros)
            {
                if (miembro.Email.ToLower().Contains(email.ToLower())) { aux.Add(miembro); }
            }
            return aux;
        }
        public List<Publicacion> BuscarPublicacionPorTexto(string texto, string email, int aceptacion)
        {
            List<Publicacion> aux = ObtenerPostPorAceptacion(texto, email, aceptacion);
            aux.AddRange(ObtenerComentariosPorAceptacion(texto, email, aceptacion));
            return aux;
        }
        public List<Publicacion> ObtenerPostPorAceptacion(string texto, string email, int aceptacion)
        {
            List<Publicacion> aux = new List<Publicacion>();
            foreach (Post post in _posts)
            {
                if (post.Texto.ToLower().Contains(texto.ToLower()) &&
                    !post.Censurado &&
                    post.CalcularAceptacion() > aceptacion &&
                    !post.Privado)
                {
                    aux.Add(post);
                }
                if (post.Texto.ToLower().Contains(texto.ToLower()) &&
                    !post.Censurado &&
                    post.CalcularAceptacion() > aceptacion &&
                    !post.Privado &&
                post.Autor.Email == email
                    && post.Privado) aux.Add(post);
            }
            aux.Sort();
            return aux;
        }
        public List<Publicacion> ObtenerComentariosPorAceptacion(string texto, string email, int aceptacion)
        {
            List<Publicacion> aux = new List<Publicacion>();
            foreach (Post post in _posts)
            {
                aux.AddRange(post.ObtenerComentarioPostPorAceptacion(texto, email, aceptacion));
            }
            aux.Sort();
            return aux;
        }

        public void CancelarEnvioSolicitudAmistad(Miembro solicitante, Miembro solicitado)
        {
            if (solicitante == null || solicitado == null)
            {
                throw new Exception("Uno de los dos miembros no existe");
            }
            if (!solicitante.ValidarAmistadPreEstablecida(solicitado))
            {
                throw new Exception($"Ya hay una amistad entre usuario email: {solicitante.Email} y {solicitado.Email}");
            }

            if (!ValidarSolicitudDeAmistad(solicitante, solicitado))
            {
                throw new Exception($"No hay una solicitud enviada de usuario email: {solicitante.Email} a {solicitado.Email}");
            }

            foreach (Solicitud solicitud in _solicitudes)
            {
                if (solicitud.EstadoSolicitud is Estado.Pendiente && solicitud.Solicitante.Email == solicitante.Email && solicitud.Solicitado.Email == solicitado.Email)
                {
                    _solicitudes.Remove(solicitud);
                }
            }
        }

        public void AceptarSolicitudAmistad(Miembro aceptante, Miembro solicitante)
        {
            if (aceptante == null || solicitante == null)
            {
                throw new Exception("Uno de los dos miembros no existe");
            }
            if (!aceptante.ValidarAmistadPreEstablecida(solicitante))
            {
                throw new Exception($"Ya hay una amistad entre usuario email: {aceptante.Email} y {solicitante.Email}");
            }
            if (!ValidarSolicitudDeAmistad(aceptante, solicitante))
            {
                throw new Exception($"No hay una solicitud enviada de usuario email: {aceptante.Email} a {solicitante.Email}");
            }
            foreach (Solicitud solicitud in _solicitudes)
            {
                if (solicitud.EstadoSolicitud is Estado.Pendiente && solicitud.Solicitante.Email == solicitante.Email && solicitud.Solicitado.Email == aceptante.Email)
                {
                    AltaNuevaAmistad(aceptante, solicitante);
                    solicitud.EstadoSolicitud = Estado.Aprobado;
                    break;
                }
            }
        }

        public void RechazarSolicitudAmistad(Miembro rechazante, Miembro solicitante)
        {
            if (rechazante == null || solicitante == null)
            {
                throw new Exception("Uno de los dos miembros no existe");
            }
            if (!rechazante.ValidarAmistadPreEstablecida(solicitante))
            {
                throw new Exception($"Ya hay una amistad entre usuario email: {rechazante.Email} y {solicitante.Email}");
            }
            if (!ValidarSolicitudDeAmistad(rechazante, solicitante))
            {
                throw new Exception($"No hay una solicitud enviada de usuario email: {rechazante.Email} a {solicitante.Email}");
            }
            foreach (Solicitud solicitud in _solicitudes)
            {
                if (solicitud.EstadoSolicitud is Estado.Pendiente && solicitud.Solicitante.Email == solicitante.Email && solicitud.Solicitado.Email == rechazante.Email)
                {
                    solicitud.EstadoSolicitud = Estado.Rechazado;
                    _solicitudes.Remove(solicitud);
                    //Si interesa guardarlas aunque esten rechazadas hay que jugar con la vista
                }
            }
        }
    }  
}
