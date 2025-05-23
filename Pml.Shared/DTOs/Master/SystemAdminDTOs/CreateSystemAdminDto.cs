using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Master.SystemAdminDTOs
{
    public class CreateSystemAdminDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // plain password
        public string UserStatus { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
    }
}
