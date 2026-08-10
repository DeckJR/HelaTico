using HelaTico.Application.DTOs;

namespace HelaTico.Web.ViewModels
{
    public class PedidoRegistroViewModel
    {
        public List<CarritoItemDTO> Carrito { get; set; } = new();

        public decimal SubTotal => Math.Round( Carrito.Sum(i => i.Precio * i.Cantidad),2);
        public decimal Impuesto =>Math.Round(SubTotal * 0.13m,2);
        public List<TipoEntregaDTO> TiposEntrega { get; set; } = new();
        public bool EsCliente { get; set; }
        public UsuarioDTO? ClienteSeleccionado { get; set; }
        public string NombreEncargado { get; set; } = string.Empty;
        public PedidoRegistroDTO Dto { get; set; } = new();
        public string? Error { get; set; }
    }
}