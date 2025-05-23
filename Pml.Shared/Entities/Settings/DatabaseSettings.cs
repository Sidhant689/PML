using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.Entities.Settings
{
    public class DatabaseSettings
    {
        public string MasterConnectionString { get; set; }
        public int CommandTimeout { get; set; } = 30;
    }
}
