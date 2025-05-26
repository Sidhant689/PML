using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.ToastMessage
{
    public class ToastMessage
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;
        public int Timeout { get; set; } = 5000;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
