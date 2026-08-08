using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HelaTico.Application.DTOs
{
    public class PedidoListaDTO
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCliente { get; set; } = null!;
        public string NombreEmpleado { get; set; } = null!;
        public string EstadoPedidoTexto { get; set; } = null!;
        public int EstadoPedido { get; set; }
        public decimal Total { get; set; }
        public int CantidadLineas { get; set; }
    }
}