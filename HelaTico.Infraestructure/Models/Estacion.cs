using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Estacion
{
    public int IdEstacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Orden> Orden { get; set; } = new List<Orden>();

    public virtual ICollection<Preparacion> Preparacion { get; set; } = new List<Preparacion>();
}
