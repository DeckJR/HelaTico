using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int IdCategoria { get; set; }

    public int EstadoProducto { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual ICollection<ComboProducto> ComboProducto { get; set; } = new List<ComboProducto>();

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Orden> Orden { get; set; } = new List<Orden>();

    public virtual ICollection<Preparacion> Preparacion { get; set; } = new List<Preparacion>();

    public virtual ICollection<Ingrediente> IdIngrediente { get; set; } = new List<Ingrediente>();

    public virtual ICollection<Menu> IdMenu { get; set; } = new List<Menu>();
}
