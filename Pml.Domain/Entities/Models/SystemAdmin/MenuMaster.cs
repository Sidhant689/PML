using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Domain.Entities.Models.Master
{
    public class MenuMaster
    {
        public string UserCode { get; set; }
        public string SerialNumber { get; set; }
        public string Module { get; set; }
        public long Code { get; set; }
        public string MenuText { get; set; }
        public string FormName { get; set; }
        public string MenuHandle { get; set; }
        public string Handle { get; set; }
    }

    public class MenuTmplate { }
    public class MenuUserTemplate { }

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
        public int SystemAdminUserId { get; set; }
        [ForeignKey("SystemAdminRole")]
        public int SystemAdminRoleId { get; set; }
        public virtual SystemAdminUser SystemAdminUser { get; set; }
        public virtual SystemAdminRole SystemAdminRole { get; set; }
    }
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
        public string TaxId { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public enum DatabaseType
    {
        SqlServer,
        Oracle,
        MsAccess,
        FoxPro,
        PostgreSQL
    }

    public class CompanyDatabase
    {
        public int Id { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DatabaseType Type { get; set; }
        public string ConnectionString { get; set; }
        public bool IsDefault { get; set; }

        public virtual Company Company { get; set; }
    }

    public class CompanyUser
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
        public virtual CompanyUserRole Role { get; set; }
    }
    public class CompanyUserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleStatus { get; set; }
    }
}
