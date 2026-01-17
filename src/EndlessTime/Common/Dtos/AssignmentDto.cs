namespace Common.Dtos
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int MaxDurtion { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CompletionDate { get; set; }
    }
}
