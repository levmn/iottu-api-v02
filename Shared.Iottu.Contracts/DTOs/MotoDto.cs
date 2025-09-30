namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Representa uma moto cadastrada no sistema.
    /// </summary>
    public class MotoDto
    {
        /// <summary>
        /// Identificador único da moto.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Placa da moto.
        /// </summary>
        public string Placa { get; set; } = string.Empty;
        /// <summary>
        /// Chassi da moto.
        /// </summary>
        public string Chassi { get; set; } = string.Empty;
        /// <summary>
        /// Número do motor.
        /// </summary>
        public string NumeroMotor { get; set; } = string.Empty;
        /// <summary>
        /// Modelo da moto.
        /// </summary>
        public required string Modelo { get; set; } = string.Empty;

        /// <summary>
        /// Id do status atual da moto.
        /// </summary>
        public int StatusId { get; set; }
        public StatusMotoDto Status { get; set; } = null!;

        /// <summary>
        /// Id da tag RFID associada.
        /// </summary>
        public Guid TagId { get; set; }
        public TagDto Tag { get; set; } = null!;

        /// <summary>
        /// Id do pátio onde a moto está alocada.
        /// </summary>
        public Guid PatioId { get; set; }
        public PatioDto Patio { get; set; } = null!;
    }

    /// <summary>
    /// Payload para criação de uma moto.
    /// </summary>
    public class CreateMotoDto
    {
        /// <summary>
        /// Placa da moto.
        /// </summary>
        public string Placa { get; set; } = string.Empty;
        /// <summary>
        /// Modelo da moto.
        /// </summary>
        public string Modelo { get; set; } = string.Empty;
        /// <summary>
        /// Chassi da moto.
        /// </summary>
        public string Chassi { get; set; } = string.Empty;
        /// <summary>
        /// Número do motor.
        /// </summary>
        public string NumeroMotor { get; set; } = string.Empty;
        /// <summary>
        /// Id do status inicial.
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Id da tag RFID associada.
        /// </summary>
        public Guid TagId { get; set; }
        /// <summary>
        /// Id do pátio associado.
        /// </summary>
        public Guid PatioId { get; set; }
    }

    /// <summary>
    /// Payload para atualização de uma moto.
    /// </summary>
    public class UpdateMotoDto
    {
        /// <summary>
        /// Modelo da moto.
        /// </summary>
        public string Modelo { get; set; } = string.Empty;
        /// <summary>
        /// Chassi da moto.
        /// </summary>
        public string Chassi { get; set; } = string.Empty;
        /// <summary>
        /// Número do motor.
        /// </summary>
        public string NumeroMotor { get; set; } = string.Empty;
        /// <summary>
        /// Id do status atualizado.
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Id da tag RFID associada.
        /// </summary>
        public Guid TagId { get; set; }
        /// <summary>
        /// Id do pátio associado.
        /// </summary>
        public Guid PatioId { get; set; }
    }
}
