using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Utilidades
    {
        public static void ComprobarTextoVaio(string texto)
        {
            if (string.IsNullOrEmpty(texto)) throw new Exception("No pueden haber campos vacios");
        }
        public static void ObjetoNulo(Object? obj)
        {
            if(obj == null) throw new Exception("Se esperaba un objeto y se recibio un nulo");
        }
        public static void ValidarFormatoEmail(string email)
        {
            int puntos = 0;
            int arroba = 0;
            for (int i = 0; i < email.Length; i++)
            {
                string unaLetra = email[i].ToString();
                if (unaLetra == ".") puntos++;
                if (unaLetra == "@") arroba++;
            }
            if (puntos == 0 || arroba != 1)
            {
                throw new Exception("Formato de email incorrecto");
            }
        }

        public static void ValidarNombreApellido(string nombreApellido)
        {
            if (string.IsNullOrWhiteSpace(nombreApellido))
            {
                throw new ArgumentException("El nombre o apellido no puede estar vacío ni contener solo espacios en blanco.");
            }

            if (!nombreApellido.All(char.IsLetter))
            {
                throw new ArgumentException("Formato incorrecto, solo puede contener letras.");
            }
        }

        public static void ValidarContraseña(string contraseña)
        {

            int Mayus = 0;
            int Num = 0;
            int Min = 0;
            for (int i = 0; i < contraseña.Length; i++)
            {
                char car = contraseña[i];
                if (char.IsUpper(car)) Mayus++;
                if (char.IsLower(car)) Min++;
                if (char.IsDigit(car)) Num++;
            }
            if (Mayus == 0 || Min == 0 || Num == 0)
            {
                throw new Exception("La contraseña debe contener al menos una mayúscula, una minúscula y un número.");
            }
        }
    }
}
