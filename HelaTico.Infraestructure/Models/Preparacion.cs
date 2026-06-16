using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Preparacion
{
    public int IdPreparacion { get; set; }

    public int IdProducto { get; set; }

    public int IdEstacion { get; set; }

    public int Orden { get; set; }

    public virtual Estacion IdEstacionNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
