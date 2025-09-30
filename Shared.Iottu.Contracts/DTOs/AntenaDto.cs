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
        public string Localizacao { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
    }

    /// <summary>
    /// Payload para atualização de antena.
    /// </summary>
    public class UpdateAntenaDto
    {
        public string Localizacao { get; set; } = string.Empty;
        public string Identificador { get; set; } = string.Empty;
    }
}
