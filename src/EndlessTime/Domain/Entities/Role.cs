namespace Domain.Entities
{
    public class Role
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<ConsultantRole> ConsultantRoles { get; set; } = new List<ConsultantRole>();

        public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}
