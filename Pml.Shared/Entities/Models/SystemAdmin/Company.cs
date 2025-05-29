using System.ComponentModel.DataAnnotations.Schema;

namespace Pml.Shared.Entities.Models.Master
{
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
}
