using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class PedidoDetalleDTO
    {
        
        //Parte para el encabezado de la facutra

        public int IdPedido { get; set; } 
        public DateTime Fecha { get; set; }
        public string EstadoPedidoTexto { get; set; } = null!;

        //Datos del cliente

        public string NombreCliente { get; set; } = null!;
        public string CorreoCliente { get; set; } = null!;

        //Empleado, nota: se usa nullable ya que no siempre se asigna un empleado a un pedido
        //ya que el propio cliente lo puede hacer

        public string? NombreEmpleado { get; set; }

        //Datos de la entrea como el tipo y si este es en el mostrador/cajas
        //entonces la direccion de entrega no se utilizará y por eso es nullable

        public string TipoEntrega { get; set; } = null!;
        public string? DireccionEntrega { get; set; }
        public decimal CostoEnvio { get; set; }


        //parte del detalle de la factura

        public List<DetallePedidoDTO> Detalle { get; set; } = new();

        //totales de lo que se le cobre por el pedido

        public decimal SubTotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal CuotaServicio { get; set; }
        public decimal Total { get; set; }

        //parte del pago, se usa nullable ya que no siempre se tiene un pago asociado a un pedido 
        //si este está pendiente

        public PagoDTO? Pago { get; set; }
    }
}
