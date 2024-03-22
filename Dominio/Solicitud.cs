using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Solicitud : IValidable
    {
        // Atributos:
        public int Id { get; private set; }
        private static int _ultId = 1;
        public Miembro Solicitante { get; set; }
        public Miembro Solicitado { get; set; }
        public Estado EstadoSolicitud { get; set; }
        public DateTime Fecha { get; set; }

        public enum Estado
        {
            Pendiente,
            Aprobado,
            Rechazado
        }

        // Constructor:
        public Solicitud(Miembro Psolicitante, Miembro Psolicitado, Estado Pestado, DateTime pFecha)
        {
            Id = _ultId++;
            Solicitante = Psolicitante;
            Solicitado = Psolicitado;
            EstadoSolicitud = Pestado;
            Fecha = pFecha;
            Validar();
        }


        public void Validar()
        {
            if (Solicitante == null || Solicitado == null) throw new Exception("Uno de los miembros en la solicitud es un null");
            if (EstadoSolicitud is not Estado.Aprobado && EstadoSolicitud is not Estado.Pendiente && EstadoSolicitud is not Estado.Rechazado)
            {
                throw new Exception("Hubo un probelma con el estado de la solicitud");
            } 
        }

    }
}
