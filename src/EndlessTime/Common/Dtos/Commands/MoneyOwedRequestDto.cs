namespace Common.Dtos.Commands
{
    public class MoneyOwedRequestDto
    {
        public Guid ConsultantId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
