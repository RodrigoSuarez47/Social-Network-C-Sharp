using Dominio;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace AppTest
{
    internal class Program
    {
        private static Sistema unSistema = Sistema.Instancia;
        static void Main(string[] args)
        {
            Bienvenida();
            int opcion = -1;
            do
            {
                Opciones();
                opcion = PedirNumero();
                bool comprobarOpcion = ComprobarOpcion(opcion);
                if (comprobarOpcion)
                {
                    switch (opcion)
                    {
                        case 1:
                            Console.WriteLine("Opcion 1:");
                            IngresarNuevoMiembro();
                            break;
                        case 2:
                            Console.WriteLine("Opcion 2:");
                            BuscarPublicacionPorEmail();
                            break;
                        case 3:
                            Console.WriteLine("Opcion 3:");
                            BuscarPostComentados();
                            break;
                        case 4:
                            Console.WriteLine("Opcion 4:");
                            MostrarPostEntreDosFechas();
                            break;
                        case 5:
                            Console.WriteLine("Opcion 5:");
                            MostrarMiembrosConMasPublicaciones();
                            break;
                        case 6:
                            PrecargarSistema();
                            break;
                        case 7:
                            Console.Clear();
                            break;
                    }
                }
                if (!comprobarOpcion)
                {
                    Console.WriteLine("Opcion fuera de rango.");
                    Console.WriteLine("###############################\n");
                }
            } while (opcion != 0);
            Console.WriteLine("Fin");
        }

        public static int PedirNumero()
        {
            int opc = -1;
            bool opcValido = false;
            do
            {
                try
                {
                    Console.WriteLine("Ingrese opcion:");
                    opc = int.Parse(Console.ReadLine());
                    opcValido = true;
                }
                catch
                {
                    Console.WriteLine("Ingrese numeros solamente");
                }
            } while (!opcValido);
            return opc;
        }
        public static bool ComprobarOpcion(int opc)
        {
            return opc >= 0 && opc <= 7;
        }
        public static void Bienvenida()
        {
            Console.WriteLine("###############################");
            Console.WriteLine("## RED SOCIAL Social.Network ##");
            Console.WriteLine("###############################");
        }
        public static void Opciones()
        {
            Console.WriteLine("\n###############################################");
            Console.WriteLine(" Opcion 1: Dar de alta un miembro.\n" +
                " Opcion 2: Listar publicaciones de un miembro\n" +
                " Opcion 3: Listar los Post en los que un miembro haya realizado comentarios\n" +
                " Opcion 4: Listar los Post realizados entre dos fechas\n" +
                " Opcion 5: Mostrar los miembros que hayan realizado mas publicaciones\n" +
                " Opcion 6: Precargar sistema\n" +
                " Opcion 7: Limpiar consola.\n" +
                " Opcion 0: Salir. \n");
            Console.WriteLine("###############################################\n");
        }
        private static void PrecargarSistema()
        {
            try
            {
                unSistema.PrecargarDatos();
                Console.WriteLine("Sistema precargado.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
        private static void IngresarNuevoMiembro()
        {
            try
            {
                Console.WriteLine("Ingrese nombre de miembro:");
                string nombre = Console.ReadLine();
                Utilidades.ValidarNombreApellido(nombre);
                Console.WriteLine("Ingrese apellido:");
                string apellido = Console.ReadLine();
                Utilidades.ValidarNombreApellido(apellido);
                Console.WriteLine("Ingrese email:");
                string email = Console.ReadLine();
                Utilidades.ValidarFormatoEmail(email);
                Console.WriteLine("Ingrese contraseña:");
                string contrasena = Console.ReadLine();
                Console.WriteLine("Ingrese fecha de nacimiento formato AAAA/MM/DD:");
                DateTime fechaNacimiento = PedirFecha();
                Miembro unMiembro = new Miembro(email, contrasena, nombre, apellido, fechaNacimiento, false);
                unSistema.CargarMiembro(unMiembro);
                Console.WriteLine($"Miembro {nombre} {apellido} cargado correctamente");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al ingresar el miembro: {e.Message}");
            }
        }
        private static int PedirNumeroFecha(string fecha)
        {
            int num = -1;
            bool valido = false;
            do
            {
                Console.WriteLine($"Ingrese {fecha} en numeros:");
                try
                {
                    num = int.Parse(Console.ReadLine());
                    valido = true;
                }
                catch
                {
                    Console.WriteLine("Ingrese numeros solamente");
                }

            } while (!valido);
            return num;
        }
        public static void MostrarMiembrosConMasPublicaciones()
        {
            try
            {
                if (unSistema.Miembros.Count > 0)
                {
                    Console.WriteLine("Miembros con mas Publicaciones: \n");
                    string respuesta = unSistema.ObtenerMiembrosConMasPublicaciones();
                    Console.WriteLine(respuesta);
                }
                else
                {
                    Console.WriteLine("No hay miembros precargados");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        public static void BuscarPublicacionPorEmail()
        {
            try
            {
                Console.WriteLine("Ingrese el correo electrónico del miembro:");
                string email = PedirEmail();

                Miembro miembroEncontrado = unSistema.BuscarMiembroPorMail(email);

                if (miembroEncontrado != null)
                {
                    Console.WriteLine($"Se ha encontrado al miembro: {miembroEncontrado.Nombre} {miembroEncontrado.Apellido}\n");

                    string publicaciones = unSistema.ListarPublicacionesDeMiembro(miembroEncontrado);

                    if (!string.IsNullOrEmpty(publicaciones))
                    {
                        Console.WriteLine(publicaciones);
                    }
                    else
                    {
                        Console.WriteLine("El miembro no tiene publicaciones.");
                    }
                }
                else
                {
                    Console.WriteLine("No se ha encontrado un miembro con el correo electrónico ingresado.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            Console.ReadKey();
        }
        public static void BuscarPostComentados()
        {
            try
            {
                string email = PedirEmail();
                Miembro miembro = unSistema.BuscarMiembroPorMail(email);
                if (miembro == null) throw new Exception("El email ingresado no pertenece a ningun usuario.");
                List<Post> postComentados = unSistema.ObtenerPostComentados(miembro.Email);
                if (postComentados.Count() > 0) ListarPosts(postComentados);
                if (postComentados.Count() == 0) Console.WriteLine("No se encontraron posts comentados");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string PedirEmail()
        {
            string email = string.Empty;
            Console.WriteLine("Ingrese email");
            email = Console.ReadLine();
            Utilidades.ValidarFormatoEmail(email);
            return email;
        }

        public static void ListarPosts(List<Post> postComentados)
        {
            foreach (Post post in postComentados)
            {
                Console.WriteLine(post);
            }
        }

        private static void MostrarPostEntreDosFechas()
        {
            try
            {
                Console.WriteLine("Fecha inicial AAAA/MM/DD:");
                DateTime fechaInicial = PedirFecha();
                Console.WriteLine($"La fecha inicial será {fechaInicial.Date}");

                Console.WriteLine("Fecha final AAAA/MM/DD:");
                DateTime fechaFinal = PedirFecha();
                Console.WriteLine($"La fecha final será {fechaFinal.Date}");

                if (fechaFinal < fechaInicial)
                {
                    Console.WriteLine("La fecha final no puede ser anterior a la fecha inicial.");
                    return;
                }

                List<Post> posts = unSistema.ObtenerPostPorFecha(fechaInicial, fechaFinal);
                string resultado = unSistema.MostrarPostsPorFecha(posts);

                Console.WriteLine("Resultados:\n");

                if (!string.IsNullOrEmpty(resultado))
                {
                    Console.WriteLine(resultado);
                }
                else
                {
                    Console.WriteLine("No se han encontrado posts con las fechas indicadas.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static DateTime PedirFecha()
        {
            try
            {
                string fechaConSlash = Console.ReadLine();
                string fechaSinSlash = SacarSlash(fechaConSlash);
                int fechaInt = int.Parse(fechaSinSlash);
                DateTime fecha = DateTime.ParseExact(fechaInt.ToString(), "yyyyMMdd", null);// Extraido de gptchat
                return fecha;

            }
            catch (Exception)
            {
                throw new Exception("La fecha ingresada no es correcta.");
            }
        }

        public static string SacarSlash(string fecha)
        {
            string resp = string.Empty;
            int slash = 0;
            foreach (char caracter in fecha)
            {
                if (char.IsDigit(caracter))
                {
                    resp += caracter;
                }
                if (caracter == '/') slash++;
            }
            if (slash == 2 && resp != string.Empty) return resp;
            return "";
        }
    }
}