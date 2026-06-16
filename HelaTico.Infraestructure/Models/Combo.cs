using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Combo
{
    public int IdCombo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public int Estado { get; set; }

    public virtual ICollection<ComboProducto> ComboProducto { get; set; } = new List<ComboProducto>();

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual ICollection<Menu> IdMenu { get; set; } = new List<Menu>();
}
