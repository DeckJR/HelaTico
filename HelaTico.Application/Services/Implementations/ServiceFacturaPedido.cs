using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;
using HelaTico.Application.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HelaTico.Application.Services.Implementations
{
    public class ServiceFacturaPedido: IServiceFacturaPedido
    {
        private readonly IServicePedido _servicePedido;


        public ServiceFacturaPedido(IServicePedido servicePedido)
        {
            _servicePedido = servicePedido;
        }


        public async Task<byte[]> GenerarFacturaAsync(int idPedido)
        {           
            var pedido = await _servicePedido.ObtenerDetalleAsync(idPedido);

            if (pedido == null)
            {
                throw new InvalidOperationException("El pedido no existe.");
            }

            QuestPDF.Settings.License =LicenseType.Community;

            var documento = Document.Create(
                container =>
                {
                    container.Page(
                        page =>
                        {                               
                            page.Size(PageSizes.A4);
                            
                            page.Margin(35);

                            page.PageColor(Colors.White);

                            page.DefaultTextStyle(x =>x.FontSize(10));

                            page.Header().Element(header =>ConstruirEncabezado(header,pedido));

                            page.Content().PaddingVertical(15).Element(content =>ConstruirContenido(content,pedido));

                            page.Footer().Element(ConstruirPie);
                        }
                    );
                }
            );


            return documento.GeneratePdf();
        }

        private static void ConstruirEncabezado(IContainer container,PedidoDetalleDTO pedido)
        {
            container.Column(
                column =>
                {
                    column.Spacing(5);

                    column.Item().Row(
                        row =>
                        {                                
                            row.RelativeItem().Column(
                                negocio =>
                                {
                                            negocio.Item().Text("HelaTico").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken3);

                                            negocio.Item().Text("Heladería").FontSize(11).FontColor(Colors.Grey.Darken1);
                                }
                            );
                            row.ConstantItem(190).AlignRight().Column(
                                datos =>
                                {
                                    datos.Item().AlignRight().Text("COMPROBANTE DE PEDIDO").FontSize(12).SemiBold();

                                    datos.Item().AlignRight().Text($"Pedido #{pedido.IdPedido}").FontSize(16).Bold().FontColor(Colors.Blue.Darken3);

                                    datos.Item().AlignRight().Text(pedido.Fecha.ToString("dd/MM/yyyy HH:mm"));
                                }
                            );
                        }
                    );
                    column.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Blue.Darken3);
                });
        }

        private static void ConstruirContenido(IContainer container,PedidoDetalleDTO pedido)
        {
            container.Column(
                column =>
                {
                    column.Spacing(15);

                    column.Item().Element(x =>ConstruirDatosGenerales(x,pedido));

                    column.Item().Element(x =>ConstruirTablaDetalle(x,pedido));

                    column.Item().AlignRight().Width(280).Element(x =>ConstruirTotales(x,pedido));

                    column.Item().Element(x =>ConstruirPago(x,pedido));
                }
            );
        }

        private static void ConstruirDatosGenerales(IContainer container,PedidoDetalleDTO pedido)
        {
            container.Row(
                row =>
                {
                    row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(
                        column =>
                        {
                            column.Spacing(4);

                            column.Item().Text("DATOS DEL CLIENTE").SemiBold().FontColor(Colors.Blue.Darken3);

                            column.Item().Text(text => {text.Span("Nombre: ").SemiBold();text.Span(pedido.NombreCliente);});

                            column.Item().Text(text =>{text.Span("Correo: ").SemiBold(); text.Span(pedido.CorreoCliente);});
                        }
                    );

                    row.ConstantItem(12);

                    row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(
                        column =>
                        {
                            column.Spacing(4);
                            
                            column.Item().Text("DATOS DEL PEDIDO").SemiBold().FontColor(Colors.Blue.Darken3);

                            column.Item().Text(text =>{text.Span("Estado: ").SemiBold();text.Span(pedido.EstadoPedidoTexto);});

                            column.Item().Text(text =>{text.Span("Entrega: ").SemiBold();text.Span(pedido.TipoEntrega);});

                            if (!string.IsNullOrWhiteSpace(pedido.DireccionEntrega))
                            {
                                column.Item().Text(text =>{text.Span("Dirección: ").SemiBold();text.Span(pedido.DireccionEntrega);});                                
                            }

                            column.Item().Text(text =>{text.Span("Atendido por: ").SemiBold();text.Span(string.IsNullOrWhiteSpace(pedido.NombreEmpleado)? "Pedido realizado por cliente": pedido.NombreEmpleado);});
                        }
                    );
                }
            );
        }

        private static void ConstruirTablaDetalle(IContainer container,PedidoDetalleDTO pedido)
        {
            container.Column(
                column =>
                {
                    column.Item().Text("DETALLE DEL PEDIDO").FontSize(12).SemiBold().FontColor(Colors.Blue.Darken3);

                    column.Item().PaddingTop(5).Table(
                        table =>
                        {
                            table.ColumnsDefinition(
                                columns =>
                                {
                                    columns.RelativeColumn(2.4f);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.8f);
                                    columns.RelativeColumn(1.3f);
                                    columns.RelativeColumn(1.2f);
                                    columns.RelativeColumn(1.3f);
                                }
                            );

                            table.Header(
                                header =>
                                {
                                    header.Cell().Element(CeldaEncabezado).Text("Artículo");

                                    header.Cell().Element(CeldaEncabezado).Text("Tipo");

                                    header.Cell().Element(CeldaEncabezado).AlignCenter().Text("Cant.");

                                    header.Cell().Element(CeldaEncabezado).AlignRight().Text("Subtotal");

                                    header.Cell().Element(CeldaEncabezado).AlignRight().Text("Impuesto");

                                    header.Cell().Element(CeldaEncabezado).AlignRight().Text("Total");
                                }
                            );
                            foreach ( var linea in pedido.Detalle)
                            {
                                table.Cell().Element(CeldaContenido).Column(
                                    item =>
                                    {
                                        item.Item().Text(linea.NombreItem).SemiBold();
                                        
                                        if (!string.IsNullOrWhiteSpace(linea.Observaciones))
                                        {
                                            item.Item().PaddingTop(2).Text($"Obs: {linea.Observaciones}").FontSize(8).FontColor(Colors.Grey.Darken1);
                                        }
                                    }
                                );
                                
                                table.Cell().Element(CeldaContenido).Text(linea.TipoItem);

                                table.Cell().Element(CeldaContenido).AlignCenter().Text(linea.Cantidad.ToString());

                                table.Cell().Element(CeldaContenido).AlignRight().Text(Moneda(linea.SubTotal));

                                table.Cell().Element(CeldaContenido).AlignRight().Text(Moneda(linea.Impuesto));

                                table.Cell().Element(CeldaContenido).AlignRight().Text(Moneda(linea.TotalLinea)).SemiBold();
                            }
                        }
                    );
                }
            );
        }

        private static void ConstruirTotales(IContainer container, PedidoDetalleDTO pedido)
        {
            container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(
                column =>
                {
                    column.Spacing(5);

                    FilaTotal(column, "Subtotal:", pedido.SubTotal);

                    FilaTotal(column, "Impuesto:", pedido.Impuesto);

                    if (pedido.CuotaServicio > 0)
                    {
                        FilaTotal(column, "Cuota de servicio:", pedido.CuotaServicio);
                    }

                    if (pedido.CostoEnvio > 0)
                    {
                        FilaTotal(column, "Costo de envío:", pedido.CostoEnvio);
                    }

                    column.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem().Text("TOTAL:").FontSize(13).Bold();

                            row.ConstantItem(110).AlignRight().Text(Moneda(pedido.Total)).FontSize(13).Bold().FontColor(Colors.Blue.Darken3);
                        }
                    );
                }
            );
        }

        private static void ConstruirPago(IContainer container,PedidoDetalleDTO pedido)
        {
            container.Column(
                column =>
                {
                    column.Item().Text("INFORMACIÓN DE PAGO").FontSize(12).SemiBold().FontColor(Colors.Blue.Darken3);

                    column.Item().PaddingTop(5).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(
                        pagoColumn =>
                        {
                            pagoColumn.Spacing(4);
                            
                            if (pedido.Pago == null)
                            {
                                pagoColumn.Item().Text("Este pedido se encuentra pendiente de pago.").FontColor(Colors.Orange.Darken2);
                                return;
                            }
                            
                            pagoColumn.Item().Text(text =>{text.Span("Método: ").SemiBold();text.Span(pedido.Pago.MetodoPagoTexto);});
                            
                            pagoColumn.Item().Text(text =>{text.Span("Monto pagado: ").SemiBold();text.Span(Moneda(pedido.Pago.Monto));});
                            
                            if (pedido.Pago.Vuelto > 0)
                            {
                                pagoColumn.Item().Text(text =>{text.Span("Vuelto: ").SemiBold();text.Span(Moneda(pedido.Pago.Vuelto));});
                            }
                            
                            pagoColumn.Item().Text(text =>{text.Span("Fecha de pago: ").SemiBold();text.Span(pedido.Pago.Fecha.ToString("dd/MM/yyyy HH:mm"));});
                        }
                    );
                }
            );
        }

        private static void ConstruirPie(IContainer container)
        {
            container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(8).Row(
                row =>
                {
                    row.RelativeItem().Text("Gracias por elegir HelaTico.").FontSize(9).FontColor(Colors.Grey.Darken1);

                    row.RelativeItem().AlignRight().DefaultTextStyle(style => style.FontSize(9).FontColor(Colors.Grey.Darken1)).Text(text =>{text.Span("Página ");text.CurrentPageNumber();text.Span(" de ");text.TotalPages();});
                }
            );
        }

        private static IContainer CeldaEncabezado(IContainer container)
        {
            return container.Background(Colors.Blue.Darken3).PaddingVertical(7).PaddingHorizontal(5).DefaultTextStyle(x =>x.FontColor(Colors.White).SemiBold().FontSize(9));
        }


        private static IContainer CeldaContenido(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(7).PaddingHorizontal(5);
        }

        private static void FilaTotal(ColumnDescriptor column,string etiqueta,decimal monto)
        {
            column.Item().Row(
                row =>
                {
                    row.RelativeItem().Text(etiqueta).FontColor(Colors.Grey.Darken2);
                    
                    row.ConstantItem(110).AlignRight().Text(Moneda(monto));
                }
            );
        }

        private static string Moneda(decimal monto)
        {
            return $"₡{monto:N2}";
        }
    }
}
