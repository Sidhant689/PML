using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Shared.Entities.Models.Client
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(20)]
        public string UserStatus { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string UserEmail { get; set; }

        [Phone]
        [StringLength(20)]
        public string UserPhone { get; set; }

        [StringLength(500)]
        public string UserAddress { get; set; }

        [ForeignKey(nameof(Role))]
        public int UserRoleCode { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(200)]
        public string RoleDescription { get; set; }

        [StringLength(20)]
        public string RoleStatus { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<User> Users { get; set; }
    }
}
