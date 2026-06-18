using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Application.DTOs;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Application.Profiles
{

    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()

                // ✅ Map básico (AutoMapper lo hace automático)
                .ForMember(dest => dest.IdCombo,
                    opt => opt.MapFrom(src => src.IdCombo))

                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))

                .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Descripcion))

                .ForMember(dest => dest.Precio,
                    opt => opt.MapFrom(src => src.Precio));

        }
    }
}
