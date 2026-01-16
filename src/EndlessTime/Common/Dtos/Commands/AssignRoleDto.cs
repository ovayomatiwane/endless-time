namespace Common.Dtos.Commands
{
    public class AssignRoleDto
    {
        public Guid RoleId { get; set; }

        public Guid ConsultantId { get; set; }

        public string? RoleName { get; set; }
    }
}
