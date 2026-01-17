namespace Common.Dtos.Commands
{
    public class AssignTaskDto
    {
        public Guid ConsultantId { get; set; }

        public Guid AssignmentId { get; set; }

        public Guid RateId { get; set; }

        public int AssignedHours { get; set; }
    }
}
