using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Master
{
    public class LookupTableDto
    {
        public string TableName { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; } // Optional: Human-readable purpose
    }
}
