using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class DetallePedido
{
    public int IdDetallePedido { get; set; }

    public int IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? IdCombo { get; set; }

    public int Cantidad { get; set; }

    public string Observaciones { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal TotalLinea { get; set; }

    public virtual Combo? IdComboNavigation { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual ICollection<Orden> Orden { get; set; } = new List<Orden>();
}
