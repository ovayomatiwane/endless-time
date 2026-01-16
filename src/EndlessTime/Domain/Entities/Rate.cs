namespace Domain.Entities
{
    public class Rate
    {
        public required Guid Id { get; set; }

        public required Guid RoleId { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsCurrent { get; set; }

        public virtual Role Role { get; set; }
    }
}
