using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PMLERP.Helpers
{
    public static class AppConfig
    {
        public static IConfiguration? Configuration { get; private set; }

        public static void Init(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}

