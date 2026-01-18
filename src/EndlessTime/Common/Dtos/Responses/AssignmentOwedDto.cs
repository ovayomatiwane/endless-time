namespace Common.Dtos.Responses
{
    public class AssignmentOwedDto
    {
        public Guid AssignmentId { get; set; }

        public string AssignmentName { get; set; } = string.Empty;

        public decimal AmountOwed { get; set; }
    }
}
