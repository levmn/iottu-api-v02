namespace Shared.Iottu.Contracts.DTOs
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public bool Ativa { get; set; }
    }

    public class CreateTagDto
    {
        public string Codigo { get; set; } = string.Empty;
    }
}
