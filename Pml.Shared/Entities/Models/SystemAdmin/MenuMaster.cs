using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.Entities.Models.Master
{
    public class MenuMaster
    {
        public long MenuCode { get; set; }
        public string Module { get; set; }
        public string MenuText { get; set; }
        public long ParCode { get; set; }
        public string? FormName { get; set; }
        public string? MenuHandle { get; set; }
        public bool AddMenuSeparator { get; set; }
        public bool HasChild { get; set; }
        public string? Lvl { get; set; }
        public int TxNType { get; set; }
        public string? ToolTip { get; set; }
        public bool IsShortcutMenuEnable { get; set; }
        public bool IsCombineTxnMenu { get; set; }
        public bool DefinedByUser { get; set; }
        public bool IsInactive { get; set; }
        public string? UrlPath { get; set; }
        public int SortOrder { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? EditedBy { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class ShortcutKeyMaster
    {
        public long KeyCode { get; set; }
        public string Description { get; set; }
        public int? ControlKey { get; set; }
        public int? Keys { get; set; }
    }
    public class MenuShortcutMapping
    {
        public long MappingId { get; set; }
        public long MenuCode { get; set; }
        public int ShortcutKey { get; set; }
        public bool IsActive { get; set; }
    }
    public class AssetMaster
    {
        public long AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? MimeType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class MenuAssetMapping
    {
        public long MappingId { get; set; }
        public long MenuCode { get; set; }
        public string? ImageName { get; set; }
        public string? SmallIcon { get; set; }
        public string? MediumIcon { get; set; }
        public string? LargeIcon { get; set; }
        public bool IsActive { get; set; }
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
