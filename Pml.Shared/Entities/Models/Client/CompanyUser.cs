using System.ComponentModel.DataAnnotations.Schema;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Shared.Entities.Models.Client
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserStatus { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        [ForeignKey("Role")]
        public int UserRoleCode { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

    }
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleStatus { get; set; }
    }
}
