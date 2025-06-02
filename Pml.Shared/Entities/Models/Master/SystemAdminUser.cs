using System.ComponentModel.DataAnnotations.Schema;

namespace Pml.Shared.Entities.Models.Master
{
    public class SystemAdminUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserStatus { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }

    public class SystemAdminRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleStatus { get; set; }
    }

    public class SystemAdminUserRole
    {
        public int Id { get; set; }
        [ForeignKey("SystemAdminUser")]
        public int VendorUserId { get; set; }
        [ForeignKey("SystemAdminRole")]
        public int VendorRoleId { get; set; }
        public virtual SystemAdminUser SystemAdminUser { get; set; }
        public virtual SystemAdminRole SystemAdminRole { get; set; }
    }
}
