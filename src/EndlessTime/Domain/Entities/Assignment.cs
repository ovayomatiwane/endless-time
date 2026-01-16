namespace Domain.Entities
{
    public class Assignment
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public int MaxDurtion { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public virtual ICollection<ConsultantAssignment> ConsultantAssignments { get; set; } = new List<ConsultantAssignment>();
    }
}
