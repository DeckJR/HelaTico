using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;
using System.Linq;

namespace HelaTico.Application.Profiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<ComboProducto, ComboProductoDTO>()
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))
                .ForMember(dest => dest.Cantidad,
                    opt => opt.MapFrom(src => src.CantidadProducto));

            CreateMap<Combo, ComboDTO>()
                .ForMember(dest => dest.Productos,
                    opt => opt.MapFrom(src => src.ComboProducto))
                .ForMember(dest => dest.CantidadProductos,
                    opt => opt.MapFrom(src =>
                        src.ComboProducto != null ? src.ComboProducto.Count : 0
                    ))
                .ForMember(dest => dest.EstadoCombo,
                    opt => opt.MapFrom(src =>
                        ((EstadoCombo)src.Estado)
                        .ToString()
                        .Replace("_", " ")
                    ))
                .ForMember(dest => dest.Imagen,
                    opt => opt.MapFrom(src => src.Imagen));
        }
    }
}