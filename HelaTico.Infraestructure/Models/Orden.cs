using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Orden
{
    public int IdOrden { get; set; }

    public int IdDetallePedido { get; set; }

    public int IdEstacion { get; set; }

    public int EstadoOrden { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int IdProducto { get; set; }

    public virtual DetallePedido IdDetallePedidoNavigation { get; set; } = null!;

    public virtual Estacion IdEstacionNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
