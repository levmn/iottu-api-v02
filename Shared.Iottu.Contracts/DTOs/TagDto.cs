using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Id da moto associada à tag.
        /// </summary>
        public Guid? MotoId { get; set; }
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
        [Required(ErrorMessage = "O Código RFID é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Código RFID deve ter no máximo 50 caracteres.")]
        public string CodigoRFID { get; set; } = string.Empty;

        [Required(ErrorMessage = "O SSID Wi-Fi é obrigatório.")]
        [StringLength(100, ErrorMessage = "O SSID Wi-Fi deve ter no máximo 100 caracteres.")]
        public string SSIDWifi { get; set; } = string.Empty;

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        [Range(-90, 90, ErrorMessage = "Latitude inválida.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        [Range(-180, 180, ErrorMessage = "Longitude inválida.")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "A data/hora é obrigatória.")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O Id da antena é obrigatório.")]
        public Guid AntenaId { get; set; }
    }

    /// <summary>
    /// Payload para atualização de uma tag.
    /// </summary>
    public class UpdateTagDto
    {
        [Required(ErrorMessage = "O Código RFID é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Código RFID deve ter no máximo 50 caracteres.")]
        public string CodigoRFID { get; set; } = string.Empty;

        [Required(ErrorMessage = "O SSID Wi-Fi é obrigatório.")]
        [StringLength(100, ErrorMessage = "O SSID Wi-Fi deve ter no máximo 100 caracteres.")]
        public string SSIDWifi { get; set; } = string.Empty;

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        [Range(-90, 90, ErrorMessage = "Latitude inválida.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        [Range(-180, 180, ErrorMessage = "Longitude inválida.")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "A data/hora é obrigatória.")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O Id da antena é obrigatório.")]
        public Guid AntenaId { get; set; }
    }
}
