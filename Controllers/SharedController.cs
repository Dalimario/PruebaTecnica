using Prueba_Tecnica.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tecnica.Controllers
{
    public class SharedController : HelpController
    {

        public SharedController(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(configuration)
        {
        }

    }
}
