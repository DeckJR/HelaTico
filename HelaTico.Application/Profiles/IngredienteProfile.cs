using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Profiles
{
    public class IngredienteProfile : Profile
    {
        public IngredienteProfile()
        {
            CreateMap<Ingrediente, IngredienteDTO>();
        }
    }
}