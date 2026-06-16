using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> IdProducto { get; set; } = new List<Producto>();
}
