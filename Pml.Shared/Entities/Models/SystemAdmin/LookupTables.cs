using System;
using System.Collections.Generic;

namespace Pml.Shared.Entities.Models.Master
{
    public static class LookupTables
    {
        public const string MenuMaster = "MenuMaster";
        public const string MenuTemplate = "MenuTemplate";
        public const string MenuUserTemplate = "MenuUserTemplate";
        public const string SystemAdminUser = "SystemAdminUser";
        public const string SystemAdminRole = "SystemAdminRole";
        public const string SystemAdminUserRole = "SystemAdminUserRole";
        public const string Company = "Company";
        public const string CompanyDatabase = "CompanyDatabase";
        public const string CompanyUser = "CompanyUser";
        public const string CompanyUserRole = "CompanyUserRole";

        public static readonly Dictionary<string, Type> EntityMap = new()
        {
            { MenuMaster, typeof(MenuMaster) },
            { MenuTemplate, typeof(MenuTmplate) },
            { MenuUserTemplate, typeof(MenuUserTemplate) },
            { SystemAdminUser, typeof(SystemAdminUser) },
            { SystemAdminRole, typeof(SystemAdminRole) },
            { SystemAdminUserRole, typeof(SystemAdminUserRole) },
            { Company, typeof(Company) },
            { CompanyDatabase, typeof(CompanyDatabase) },
            { CompanyUser, typeof(CompanyUser) },
            { CompanyUserRole, typeof(CompanyUserRole) },
        };
    }
}
