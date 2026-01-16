namespace Common.Dtos
{
    public class ConsultantRoleDto
    {
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public Guid ConsultantId { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime CreatedDate { get; set; }

        public RoleDto? Role { get; set; }

        public ConsultantDto? Consultant { get; set; }
    }
}
