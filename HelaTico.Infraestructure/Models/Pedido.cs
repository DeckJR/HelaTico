using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime Fecha { get; set; }

    public int IdTipoEntrega { get; set; }

    public string DireccionEntrega { get; set; } = null!;

    public decimal CostoEnvio { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public int EstadoPedido { get; set; }

    public decimal CuotaServicio { get; set; }

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual ICollection<HistorialEstadoPedido> HistorialEstadoPedido { get; set; } = new List<HistorialEstadoPedido>();

    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdEmpleadoNavigation { get; set; } = null!;

    public virtual TipoEntrega IdTipoEntregaNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pago { get; set; } = new List<Pago>();
}
