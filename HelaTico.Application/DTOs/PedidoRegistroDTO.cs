using System.ComponentModel.DataAnnotations;

namespace HelaTico.Application.DTOs
{
    public class PedidoRegistroDTO
    {
        // el Id del cliente se tiene por si el que registra/hace el pedido es un encargado o administrador
        public int IdCliente { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "Debe seleccionar un método de entrega.")]
        public int IdTipoEntrega { get; set; }

        [StringLength(
            150,
            ErrorMessage =
                "La dirección no puede superar 150 caracteres.")]
        public string? DireccionEntrega { get; set; }
    }
}