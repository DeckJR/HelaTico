using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceProducto
    {
        Task<ICollection<ProductoDTO>> ListAsync();
        Task<ProductoDTO> FindByIdAsync(int id);

    }
}
