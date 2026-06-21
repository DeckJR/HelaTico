using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;
using System.Linq;

namespace HelaTico.Application.Profiles
{
    public class PreparacionProfile : Profile
    {
        public PreparacionProfile()
        {
            CreateMap<Preparacion, PasoPreparacionDTO>()
                .ForMember(dest => dest.NombreEstacion,
                    opt => opt.MapFrom(src => src.IdEstacionNavigation.Descripcion))
                .ForMember(dest => dest.Orden,
                    opt => opt.MapFrom(src => src.Orden));

            CreateMap<Producto, PreparacionDTO>()
                .ForMember(dest => dest.IdProducto,
                    opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.CantidadPasos,
                    opt => opt.MapFrom(src => src.Preparacion.Count))
                .ForMember(dest => dest.Pasos,
                    opt => opt.MapFrom(src => src.Preparacion.OrderBy(p => p.Orden)));
        }
    }
}
