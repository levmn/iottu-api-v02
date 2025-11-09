using System.ComponentModel.DataAnnotations;

namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Representa uma antena leitora de tags.
    /// </summary>
    public class AntenaDto
    {
        /// <summary>
        /// Identificador único da antena.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Localização descritiva da antena.
        /// </summary>
        public string Localizacao { get; set; } = string.Empty;
        /// <summary>
        /// Identificador legível da antena.
        /// </summary>
        public string Identificador { get; set; } = string.Empty;
    }

    /// <summary>
    /// Payload para criação de antena.
    /// </summary>
    public class CreateAntenaDto
    {
        [Required(ErrorMessage = "A localização é obrigatória.")]
        [StringLength(100, ErrorMessage = "A localização deve ter no máximo 100 caracteres.")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O identificador é obrigatório.")]
        [StringLength(50, ErrorMessage = "O identificador deve ter no máximo 50 caracteres.")]
        public string Identificador { get; set; } = string.Empty;
    }

    /// <summary>
    /// Payload para atualização de antena.
    /// </summary>
    public class UpdateAntenaDto
    {
        [Required(ErrorMessage = "A localização é obrigatória.")]
        [StringLength(100, ErrorMessage = "A localização deve ter no máximo 100 caracteres.")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O identificador é obrigatório.")]
        [StringLength(50, ErrorMessage = "O identificador deve ter no máximo 50 caracteres.")]
        public string Identificador { get; set; } = string.Empty;
    }
}
