using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pml.Shared.Entities.Models.Master
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string CompanyCode { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Logo { get; set; }

        public string TaxId { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public ICollection<CompanyDatabase> CompanyDatabases { get; set; }
    }

    public enum DatabaseType
    {
        SqlServer,
        Oracle,
        MsAccess,
        FoxPro,
        PostgreSQL,
        SQLite,
    }

    public class CompanyDatabase
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public string Name { get; set; }

        public DatabaseType Type { get; set; }

        public string ConnectionString { get; set; }

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }
}
