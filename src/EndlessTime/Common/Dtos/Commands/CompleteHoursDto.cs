namespace Common.Dtos.Commands
{
    public class CompleteHoursDto
    {
        public Guid ConsultantAssignmentId { get; set; }

        public int HoursCompleted { get; set; }
    }
}
