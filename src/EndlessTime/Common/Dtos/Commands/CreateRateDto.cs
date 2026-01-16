namespace Common.Dtos.Commands
{
    public class CreateRateDto
    {
        public Guid RoleId { get; set; }

        public decimal HourlyRate { get; set; }
    }
}
