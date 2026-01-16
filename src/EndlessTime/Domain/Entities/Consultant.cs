namespace Domain.Entities
{
    public class Consultant
    {
        public required Guid Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageFileName { get; set; }

        public virtual ICollection<ConsultantRole> ConsultantRoles { get; set; } = new List<ConsultantRole>();

        public virtual ICollection<ConsultantAssignment> ConsultantAssignments { get; set; } = new List<ConsultantAssignment>();
    }
}
