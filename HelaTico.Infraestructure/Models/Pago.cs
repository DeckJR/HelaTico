using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdPedido { get; set; }

    public int MetodoPago { get; set; }

    public decimal Monto { get; set; }

    public decimal Vuelto { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
