using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinal { get; set; }

    public int EstadoMenu { get; set; }

    public virtual ICollection<Combo> IdCombo { get; set; } = new List<Combo>();

    public virtual ICollection<Producto> IdProducto { get; set; } = new List<Producto>();
}
