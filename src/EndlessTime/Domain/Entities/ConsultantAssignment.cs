namespace Domain.Entities
{
    public class ConsultantAssignment
    {
        public required Guid Id { get; set; }

        public required Guid RateId { get; set; }

        public required Guid ConsultantId { get; set; }

        public required Guid AssignmentId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int HoursAssigned { get; set; }

        public int HoursCompleted { get; set; }

        public virtual Rate Rate { get; set; }

        public virtual Consultant Consultant { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
