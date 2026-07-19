using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;
using System.Linq;

namespace HelaTico.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>()

          
                .ForMember(dest => dest.Categoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Descripcion))


                 .ForMember(dest => dest.IdIngrediente,
                    opt => opt.MapFrom(src => src.IdIngrediente.Select(i => i.IdIngrediente).ToArray()))

                .ForMember(dest => dest.Ingredientes,
                    opt => opt.MapFrom(src =>
                        src.IdIngrediente != null
                            ? src.IdIngrediente.Select(i => i.Descripcion).ToList()
                            : new List<string>()
                    ))

                .ForMember(dest => dest.EstadoProductoId,
                    opt => opt.MapFrom(src => src.EstadoProducto))

                .ForMember(dest => dest.EstadoProducto,
                    opt => opt.MapFrom(src =>
                        ((EstadoProducto)src.EstadoProducto)
                        .ToString()
                        .Replace("_", " ")
                    ))

                .ForMember(dest => dest.Imagen,
                    opt => opt.MapFrom(src => src.Imagen))

                .ReverseMap()
                .ForMember(dest => dest.EstadoProducto, opt => opt.MapFrom(src => src.EstadoProductoId))
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdIngrediente, opt => opt.Ignore())
                .ForMember(dest => dest.ComboProducto, opt => opt.Ignore())
                .ForMember(dest => dest.DetallePedido, opt => opt.Ignore())
                .ForMember(dest => dest.Orden, opt => opt.Ignore())
                .ForMember(dest => dest.Preparacion, opt => opt.Ignore())
                .ForMember(dest => dest.IdMenu, opt => opt.Ignore());
        }
    }
}