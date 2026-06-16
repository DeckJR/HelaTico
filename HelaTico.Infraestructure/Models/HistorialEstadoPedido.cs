using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class HistorialEstadoPedido
{
    public int IdHistorial { get; set; }

    public int IdPedido { get; set; }

    public int EstadoPedido { get; set; }

    public DateTime FechaYhora { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
