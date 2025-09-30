namespace Shared.Iottu.Contracts.DTOs
{
    public class AntenaDto
    {
        public Guid Id { get; set; }
        public string Localizacao { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
    }

    public class CreateAntenaDto
    {
        public string Localizacao { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
    }

    public class UpdateAntenaDto
    {
        public string Localizacao { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
    }
}
