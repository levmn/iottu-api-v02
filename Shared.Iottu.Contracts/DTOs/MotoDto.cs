namespace Shared.Iottu.Contracts.DTOs
{
    public class MotoDto
    {
        public Guid Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public required string Modelo { get; set; } = string.Empty;

        public int StatusId { get; set; }
        public StatusMotoDto Status { get; set; } = null!;

        public Guid TagId { get; set; }
        public TagDto Tag { get; set; } = null!;

        public Guid PatioId { get; set; }
        public PatioDto Patio { get; set; } = null!;
    }

    public class CreateMotoDto
    {
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public Guid TagId { get; set; }
        public Guid PatioId { get; set; }
    }

    public class UpdateMotoDto
    {
        public string Modelo { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public Guid TagId { get; set; }
        public Guid PatioId { get; set; }
    }
}
