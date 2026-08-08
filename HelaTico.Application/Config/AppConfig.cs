using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.Config
{
    public class AppConfig
    {
        public Crypto Crypto { get; set; } = default!;
    }

    public class Crypto
    {
        public string Secret { get; set; } = default!;
    }
}
