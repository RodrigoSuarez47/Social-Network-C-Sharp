using Microsoft.AspNetCore.Mvc;
using Dominio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata.Ecma335;
using AppWeb.Filtros;

namespace AppWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private Sistema _sistema = Sistema.Instancia;
        public IActionResult Index()
        {
            string usuarioIngresado = HttpContext.Session.GetString("usuarioIngresado");
            string rol = HttpContext.Session.GetString("rol");
            string bloqueado = HttpContext.Session.GetString("bloqueado");
            return View();
        }

        public IActionResult ListarMiembros()
        {
            try
            {
                ViewBag.miembros = _sistema.ListarMiembrosOrdenados();
                ViewBag.solicitudes = _sistema.Solicitudes;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }

        [Admin]
        public IActionResult BloquearMiembro(string email)
        {
            try
            {
                Miembro miembro = _sistema.BuscarMiembroPorMail(email);
                if (miembro == null) throw new Exception($"El email {email} no existe en la base de datos");
                miembro.Bloqueado = true;
                return RedirectToAction("ListarMiembrosAdmin");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            var miembrosOrdenados = _sistema.Miembros.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToList();
            @ViewBag.miembros = miembrosOrdenados;
            return View("index");
        }

        [Admin]
        public IActionResult DesbloquearMiembro(string email)
        {
            try
            {
                Miembro miembro = _sistema.BuscarMiembroPorMail(email.ToLower());
                if (miembro == null) throw new Exception($"El email {email} no existe en la base de datos");
                miembro.Bloqueado = false;
                return RedirectToAction("ListarMiembrosAdmin");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            var miembrosOrdenados = _sistema.Miembros.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToList();
            @ViewBag.miembros = miembrosOrdenados;
            return View("index");
        }

        public IActionResult RegistrarMiembro()
        {
            return View(new Miembro());
        }

        [HttpPost]
        public IActionResult RegistrarMiembro(Miembro miembro)
        {
            try
            {
                _sistema.CargarMiembro(miembro);
                ViewBag.mensaje = $"Se ha registrado correctamente";
                return View(new Miembro());
            }
            catch (Exception error)
            {
                ViewBag.error = error.Message;
            }
            return View(miembro);
        }

        public IActionResult IngresarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IngresarUsuario(string email, string contraseña)
        {
            try
            {
                Usuario usuario = _sistema.BuscarUsuarioPorEmail(email.ToLower());
                if (usuario == null) throw new Exception("El email o contraseña son incorrectos.");
                if (usuario.Contraseña != contraseña) throw new Exception("El email o contraseña son incorrectos.");
                string rol = string.Empty;
                string bloqueado = string.Empty;
                if (usuario is Miembro)
                {
                    rol = "miembro";
                    HttpContext.Session.SetString("rol", rol);
                    HttpContext.Session.SetString("usuarioIngresado", email.ToLower());
                    Miembro usuarioEncontrado = (usuario as Miembro);
                    if (usuarioEncontrado.Bloqueado)
                    {
                        bloqueado = "bloqueado";
                    }
                    else if (!usuarioEncontrado.Bloqueado)
                    {
                        bloqueado = "desbloqueado";
                    }
                    HttpContext.Session.SetString("bloqueado", bloqueado);
                    return Redirect("/Publicacion/VerPublicaciones");
                }
                if (usuario is Administrador)
                {
                    bloqueado = "desbloqueado";
                    rol = "administrador";
                    HttpContext.Session.SetString("rol", rol);
                    HttpContext.Session.SetString("usuarioIngresado", email.ToLower());
                    HttpContext.Session.SetString("bloqueado", bloqueado);
                    return Redirect("/Publicacion/VerPublicaciones");
                }

            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("IngresarUsuario");
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [Miemb]
        public IActionResult EnviarSolicitudAmistad(string email)
        {
            try
            {
                Miembro solicitante = _sistema.BuscarMiembroPorMail(HttpContext.Session.GetString("usuarioIngresado"));
                Miembro solicitado = _sistema.BuscarMiembroPorMail(email.ToLower());
                if (solicitado != null && solicitante != null)
                {
                    if (!_sistema.ValidarSolicitudDeAmistad(solicitante, solicitado))
                    {
                        _sistema.EnviarSolicitudAmistad(solicitante, solicitado);
                        return RedirectToAction("ListarMiembros");
                    }
                    else
                    {
                        return RedirectToAction("ListarMiembros");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("ListarMiembros");
        }
        [Miemb]
        public IActionResult CancelarSolicitudAmistad(string email)
        {
            try
            {
                Miembro solicitante = _sistema.BuscarMiembroPorMail(HttpContext.Session.GetString("usuarioIngresado"));
                Miembro solicitado = _sistema.BuscarMiembroPorMail(email.ToLower());
                if (solicitado != null && solicitante != null)
                {
                    if (_sistema.ValidarSolicitudDeAmistad(solicitante, solicitado))
                    {
                        _sistema.CancelarEnvioSolicitudAmistad(solicitante, solicitado);
                        return RedirectToAction("ListarMiembros");
                    }
                    else
                    {
                        return RedirectToAction("ListarMiembros");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("ListarMiembros");
        }

        [Admin]
        public IActionResult ListarMiembrosAdmin()
        {
            try
            {
                ViewBag.miembros = _sistema.ListarMiembrosOrdenados();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }
        [HttpPost]
        [Admin]
        public IActionResult ListarMiembrosAdmin(string email)
        {
            try
            {
                if (email == null) return RedirectToAction("ListarMiembrosAdmin");
                ViewBag.miembros = _sistema.BuscarMiembroConEmail(email.ToLower());                
                return View("ListarMiembrosAdmin");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View();
        }

        [Miemb]
        public IActionResult AceptarSolicitudAmistad(string email)
        {
            try
            {
                Miembro aceptante = _sistema.BuscarMiembroPorMail(HttpContext.Session.GetString("usuarioIngresado"));
                Miembro solicitante = _sistema.BuscarMiembroPorMail(email.ToLower());
                if (aceptante != null && solicitante != null)
                {
                    if (_sistema.ValidarSolicitudDeAmistad(aceptante, solicitante))
                    {
                        _sistema.AceptarSolicitudAmistad(aceptante, solicitante);
                        return RedirectToAction("ListarMiembros");
                    }
                    else
                    {
                        return RedirectToAction("ListarMiembros");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("ListarMiembros");
        }

        [Miemb]
        public IActionResult RechazarSolicitudAmistad (string email)
        {
            try
            {
                Miembro rechazante = _sistema.BuscarMiembroPorMail(HttpContext.Session.GetString("usuarioIngresado"));
                Miembro solicitante = _sistema.BuscarMiembroPorMail(email.ToLower());
                if (rechazante != null && solicitante != null)
                {
                    if (_sistema.ValidarSolicitudDeAmistad(rechazante, solicitante))
                    {
                        _sistema.RechazarSolicitudAmistad(rechazante, solicitante);
                        return RedirectToAction("ListarMiembros");
                    }
                    else
                    {
                        return RedirectToAction("ListarMiembros");
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return RedirectToAction("ListarMiembros");
        }
    }
}
