namespace Common.Dtos
{
    public class RateDto
    {
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsCurrent { get; set; }

        public virtual RoleDto? Role { get; set; }
    }
}
