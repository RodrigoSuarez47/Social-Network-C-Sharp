using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using AppWeb.Filtros;
using System.Runtime.CompilerServices;

namespace AppWeb.Controllers
{
    public class PublicacionController : Controller
    {

        private Sistema _sistema = Sistema.Instancia;
        public IActionResult VerPublicaciones()
        {
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado").ToLower() ?? "";
                string rol = HttpContext.Session.GetString("rol") ?? "";
                if(rol == "miembro")ViewBag.misPublicaciones = _sistema.ObtenerMisPublicaciones(UsuarioLoggeado);
                if(rol == "administrador") ViewBag.misPublicaciones = _sistema.ObtenerPublicacionesAdministrador();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult VerComentariosPost(int postId)
        {
            try
            { 
                ViewBag.ComentariosDePost = _sistema.ObtenerComentariosDePostId(postId);
                ViewBag.Post=_sistema.ObtenerPostXId(postId);
                return View();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult VerReacciones(int postId)
        {
            try
            {
                ViewBag.post = _sistema.ObtenerPostXId(postId);
                Post post = _sistema.ObtenerPostXId(postId);
                ViewBag.likes = post.CalcularLikes();
                ViewBag.disLikes = post.CalcularDisLikes();
                ViewBag.VA = post.CalcularAceptacion();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult AnadirMeGusta(int PostId)
        {
           
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                string accion = "like";
                _sistema.AnadirReaccion(PostId, UsuarioLoggeado, accion);
                return RedirectToAction("VerReacciones", new { postId = PostId });
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("VerReacciones", new { postId = PostId });
        }
        [Miemb]
        public IActionResult AnadirNoMeGusta(int PostId)
        {
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                string accion = "dislike";
                _sistema.AnadirReaccion(PostId, UsuarioLoggeado, accion);
                return RedirectToAction("VerReacciones", new { postId = PostId });
            }
            catch (Exception error)
            {
                ViewBag.error = error.Message; 
            }
            return RedirectToAction("VerReacciones", new { postId = PostId });
        }
        [Miemb]
        public IActionResult VerLikes(int postId)
        {
            try
            {
                ViewBag.Lista = _sistema.ObtenerListaLikesPost(postId);
                ViewBag.post= _sistema.ObtenerPostXId(postId);
                ViewBag.Accion = " me gusta";
                return View();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult VerdisLikes(int postId)
        {
            try
            {
                ViewBag.Lista = _sistema.ObtenerListaDisLikesPost(postId);
                ViewBag.post = _sistema.ObtenerPostXId(postId);
                ViewBag.Accion = " no me gusta";
                return View("VerLikes");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("VerLikes");
        }
        [Miemb]
        public IActionResult AñadirComentario(int postId)
        {
            ViewBag.Post = _sistema.ObtenerPostXId(postId);
            return View(new Comentario());
        }

        [Miemb]
        [HttpPost]
        public IActionResult AñadirComentario(int postId, Comentario comentario)
        {
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                ViewBag.postId = postId;
                Miembro miembro = _sistema.BuscarMiembroPorMail(UsuarioLoggeado);
                comentario.Autor = miembro;
                comentario.FechaPublicacion = DateTime.Now;
                _sistema.AñadirComentario(postId, comentario);
                return RedirectToAction("VerComentariosPost", new { postId = postId });
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("AñadirComentario", new { postId = postId });
        }
        [Miemb]
        public IActionResult VerReaccionesComentario(int comentarioId)
        {
            try
            {
                Comentario comentario = _sistema.ObtenerComentarioId(comentarioId);
                ViewBag.Comentario = comentario;
                ViewBag.post = _sistema.ObtenerPostPorComentario(comentario);
                ViewBag.likes = comentario.CalcularLikes();
                ViewBag.disLikes = comentario.CalcularDisLikes();
                ViewBag.VA = comentario.CalcularAceptacion();
                return View();
            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult AnadirMeGustaComentario(int comentarioId)
        {
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                string accion = "like";
                _sistema.AnadirReaccionComentario(comentarioId, UsuarioLoggeado, accion);
                return RedirectToAction("VerReaccionesComentario", new { comentarioId= comentarioId });
            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("VerReaccionesComentario", new { comentarioId = comentarioId });
        }
        [Miemb]
        public IActionResult AnadirNoMeGustaComentario(int comentarioId)
        {
            try
            {
                string UsuarioLoggeado = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                string accion = "dislike";
                _sistema.AnadirReaccionComentario(comentarioId, UsuarioLoggeado, accion);
                return RedirectToAction("VerReaccionesComentario", new { comentarioId = comentarioId });
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("VerReaccionesComentario", new { comentarioId = comentarioId });
        }
        [Miemb]
        public IActionResult VerLikesComentario(int comentarioId)
        {
            try
            {
                ViewBag.VerLikes = _sistema.ObtenerLikesComentario(comentarioId);
                ViewBag.Accion = "me gusta";
                ViewBag.comentarioId= comentarioId;
                return View();
            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [Miemb]
        public IActionResult VerdisLikesComentario(int comentarioId)
        {
            try
            {
                ViewBag.VerLikes = _sistema.ObtenerdisLikesComentario(comentarioId);
                ViewBag.Accion = " no me gusta";
                ViewBag.comentarioId = comentarioId;
                return View("VerLikesComentario");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("VerLikesComentario");
        }

        [Admin]
        public IActionResult BanearPost(int postId)
        {
            try
            {
                Post postEncontrado = _sistema.ObtenerPostXId(postId);
                if (postEncontrado == null) throw new Exception("No se econtraron post");
                postEncontrado.Censurado = true;
                return RedirectToAction("VerPublicaciones"); 
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("Index");
        }


        [Admin]
        public IActionResult DesbanearPost(int postId)
        {
            try
            {
                Post postEncontrado = _sistema.ObtenerPostXId(postId);
                if (postEncontrado == null) throw new Exception("No se econtraron post");
                postEncontrado.Censurado = false;
                return RedirectToAction("VerPublicaciones");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("Index");
        }

        [Miemb]
        public IActionResult CrearPost()
        {
            return View(new Post());
        }
        [Miemb]
        [HttpPost]
        public IActionResult CrearPost(Post post)
        {
            try
            {
                string email = HttpContext.Session.GetString("usuarioIngresado") ?? "";
                Miembro miembro = _sistema.BuscarMiembroPorMail(email);
                if (miembro == null) throw new Exception("El email buscado no existe");
                post.Autor=miembro;
                post.FechaPublicacion= DateTime.Now;
                _sistema.ValidarPublicacion(post);
                return RedirectToAction("VerPublicaciones");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View(new Post());
        }

        public IActionResult BuscarPublicacionPorAceptacion()
        {
            return View();
        }
        [HttpPost]
        [Miemb]
        public IActionResult BuscarPublicacionPorAceptacion(string textoPublicacion, int valorAceptacion)
        {
            try
            {
                if (string.IsNullOrEmpty(textoPublicacion)) return RedirectToAction("VerPublicaciones");
                string email = HttpContext.Session.GetString("usuarioIngresado");
                ViewBag.misPublicaciones= _sistema.BuscarPublicacionPorTexto(textoPublicacion, email, valorAceptacion);  
                if (ViewBag.misPublicaciones.Count == 0)
                {
                    ViewBag.error = "No se encontraron publicaciones";
                    return View("BuscarPublicacionPorAceptacion");
                }

                return View("BuscarPublicacionPorAceptacion");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("BuscarPublicacionPorAceptacion");
        }
    }
}
