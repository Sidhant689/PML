namespace Pml.Shared.DTOs.Client
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleStatus { get; set; }
        public bool IsActive { get; set; }
    }

}
