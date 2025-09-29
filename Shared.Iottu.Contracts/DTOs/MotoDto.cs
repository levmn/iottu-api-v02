namespace Shared.Iottu.Contracts.DTOs
{
    public class MotoDto
    {
        public Guid Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public Guid TagId { get; set; }
    }

    public class CreateMotoDto
    {
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public Guid TagId { get; set; }
    }

    public class UpdateMotoDto
    {
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
    }
}
