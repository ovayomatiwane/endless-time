namespace Domain.Entities
{
    public class ConsultantRole
    {
        public required Guid Id { get; set; }

        public required Guid RoleId { get; set; }

        public required Guid ConsultantId { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Role Role { get; set; }

        public virtual Consultant Consultant { get; set; }
    }
}
