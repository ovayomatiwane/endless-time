namespace Common.Dtos.Responses
{
    public class MoneyOwedDto
    {
        public string FullName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public decimal TotalOwed { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<AssignmentOwedDto> AssignmentsOwed { get; set; } = [];
    }
}
