namespace Common.Dtos
{
    public class ConsultantAssignmentDto
    {
        public Guid Id { get; set; }

        public Guid RateId { get; set; }

        public Guid ConsultantId { get; set; }

        public Guid AssignmentId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int HoursAssigned { get; set; }

        public int HoursCompleted { get; set; }

        public virtual RateDto Rate { get; set; }

        public virtual ConsultantDto Consultant { get; set; }

        public virtual AssignmentDto Assignment { get; set; }
    }
}
