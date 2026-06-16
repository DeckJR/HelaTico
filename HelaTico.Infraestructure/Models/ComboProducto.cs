using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class ComboProducto
{
    public int IdCombo { get; set; }

    public int IdProducto { get; set; }

    public int CantidadProducto { get; set; }

    public virtual Combo IdComboNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
