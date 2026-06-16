using System;
using System.Collections.Generic;

namespace HelaTico.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasenna { get; set; } = null!;

    public int IdRolUsuario { get; set; }

    public int EstadoUsuario { get; set; }

    public virtual RolUsuario IdRolUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> PedidoIdClienteNavigation { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoIdEmpleadoNavigation { get; set; } = new List<Pedido>();
}
