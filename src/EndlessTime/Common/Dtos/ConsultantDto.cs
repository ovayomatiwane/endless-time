namespace Common.Dtos
{
    public class ConsultantDto
    {
        public required Guid Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageFileName { get; set; }
    }
}
