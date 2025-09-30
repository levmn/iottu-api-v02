namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Representa uma tag de rastreamento (RFID/Wi-Fi) associada a uma moto/antena.
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Identificador único da tag.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Código RFID lido pela antena.
        /// </summary>
        public required string CodigoRFID { get; set; }
        /// <summary>
        /// SSID da rede Wi-Fi detectada.
        /// </summary>
        public required string SSIDWifi { get; set; }
        /// <summary>
        /// Latitude da leitura.
        /// </summary>
        public required double Latitude { get; set; }
        /// <summary>
        /// Longitude da leitura.
        /// </summary>
        public required double Longitude { get; set; }
        /// <summary>
        /// Data/hora da leitura (UTC).
        /// </summary>
        public required DateTime DataHora { get; set; }

        public MotoDto? Moto { get; set; }

        /// <summary>
        /// Id da antena que realizou a leitura.
        /// </summary>
        public Guid AntenaId { get; set; }
        public AntenaDto? Antena { get; set; }

    }

    /// <summary>
    /// Payload para criação de uma tag.
    /// </summary>
    public class CreateTagDto
    {
        public string CodigoRFID { get; set; } = string.Empty;
        public string SSIDWifi { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public Guid AntenaId { get; set; }
    }

    /// <summary>
    /// Payload para atualização de uma tag.
    /// </summary>
    public class UpdateTagDto
    {
        public string CodigoRFID { get; set; } = string.Empty;
        public string SSIDWifi { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public Guid AntenaId { get; set; }
    }
}
