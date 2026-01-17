namespace Common.Dtos.Commands
{
    public class CreateAssignmentDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public int MaxDuration { get; set; }
    }
}
