using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
