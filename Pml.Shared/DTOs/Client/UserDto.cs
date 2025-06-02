using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Client
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserStatus { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public int UserRoleCode { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }

    public class CreateUserDto
    {
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

        [Required]
        public int UserRoleCode { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string UserEmail { get; set; }

        [Phone]
        [StringLength(20)]
        public string UserPhone { get; set; }

        [StringLength(500)]
        public string UserAddress { get; set; }

        public string UserStatus { get; set; }

        [Required]
        public int UserRoleCode { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public bool IsActive { get; set; }
    }

}
