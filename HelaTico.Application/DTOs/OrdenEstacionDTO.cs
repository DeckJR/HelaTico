using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class OrdenEstacionDTO
    {
        public int IdOrden { get; set; }
        public int IdPedido { get; set; }
        public int IdDetallePedido { get; set; }
        public int IdEstacion { get; set; }
        public string Estacion { get; set; } = string.Empty;
        public int IdProducto { get; set; }
        public string Producto { get; set; }= string.Empty;
        public int Cantidad { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int EstadoOrden { get; set; }
        public string EstadoOrdenTexto { get; set; }= string.Empty;
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
    }
}
