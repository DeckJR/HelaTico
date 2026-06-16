using System;
using System.Collections.Generic;
using HelaTico.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace HelaTico.Infraestructure.Data;

public partial class HelaTicoContext : DbContext
{
    public HelaTicoContext(DbContextOptions<HelaTicoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Combo> Combo { get; set; }

    public virtual DbSet<ComboProducto> ComboProducto { get; set; }

    public virtual DbSet<DetallePedido> DetallePedido { get; set; }

    public virtual DbSet<Estacion> Estacion { get; set; }

    public virtual DbSet<HistorialEstadoPedido> HistorialEstadoPedido { get; set; }

    public virtual DbSet<Ingrediente> Ingrediente { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Orden> Orden { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<Preparacion> Preparacion { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<RolUsuario> RolUsuario { get; set; }

    public virtual DbSet<TipoEntrega> TipoEntrega { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.IdCombo);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<ComboProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdCombo, e.IdProducto });

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.ComboProducto)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Combo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ComboProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Producto1");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido);

            entity.Property(e => e.Impuesto).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Observaciones).HasMaxLength(50);
            entity.Property(e => e.SubTotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TotalLinea).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdCombo)
                .HasConstraintName("FK_DetallePedido_Combo");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallePedido_Pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_DetallePedido_Producto");
        });

        modelBuilder.Entity<Estacion>(entity =>
        {
            entity.HasKey(e => e.IdEstacion);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<HistorialEstadoPedido>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.Property(e => e.FechaYhora)
                .HasColumnType("datetime")
                .HasColumnName("FechaYHora");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.HistorialEstadoPedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstadoPedido_Pedido");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu);

            entity.Property(e => e.FechaFinal).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.IdCombo).WithMany(p => p.IdMenu)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuCombo",
                    r => r.HasOne<Combo>().WithMany()
                        .HasForeignKey("IdCombo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuCombo_Combo"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("IdMenu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuCombo_Menu"),
                    j =>
                    {
                        j.HasKey("IdMenu", "IdCombo");
                    });

            entity.HasMany(d => d.IdProducto).WithMany(p => p.IdMenu)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuProducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuProducto_Producto"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("IdMenu")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MenuProducto_Menu"),
                    j =>
                    {
                        j.HasKey("IdMenu", "IdProducto");
                    });
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.IdOrden);

            entity.HasOne(d => d.IdDetallePedidoNavigation).WithMany(p => p.Orden)
                .HasForeignKey(d => d.IdDetallePedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orden_DetallePedido");

            entity.HasOne(d => d.IdEstacionNavigation).WithMany(p => p.Orden)
                .HasForeignKey(d => d.IdEstacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orden_Estacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Orden)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orden_Producto");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago);

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Vuelto).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Pedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido);

            entity.Property(e => e.CostoEnvio).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CuotaServicio).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.DireccionEntrega).HasMaxLength(150);
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Impuesto).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Total).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PedidoIdClienteNavigation)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.PedidoIdEmpleadoNavigation)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Empleado");

            entity.HasOne(d => d.IdTipoEntregaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdTipoEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_TipoEntrega");
        });

        modelBuilder.Entity<Preparacion>(entity =>
        {
            entity.HasKey(e => e.IdPreparacion);

            entity.Property(e => e.Orden).HasColumnName("orden");

            entity.HasOne(d => d.IdEstacionNavigation).WithMany(p => p.Preparacion)
                .HasForeignKey(d => d.IdEstacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Preparacion_Estacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Preparacion)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Preparacion_Producto");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasMany(d => d.IdIngrediente).WithMany(p => p.IdProducto)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductoIngrediente",
                    r => r.HasOne<Ingrediente>().WithMany()
                        .HasForeignKey("IdIngrediente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoIngrediente_Ingrediente"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoIngrediente_Producto"),
                    j =>
                    {
                        j.HasKey("IdProducto", "IdIngrediente");
                    });
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => e.IdRolUsuario);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoEntrega>(entity =>
        {
            entity.HasKey(e => e.IdTipoEntrega);

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.Property(e => e.Apellido1).HasMaxLength(50);
            entity.Property(e => e.Apellido2).HasMaxLength(50);
            entity.Property(e => e.Contrasenna).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.IdRolUsuarioNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRolUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_RolUsuario1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
