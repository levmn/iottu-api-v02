namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Representa um pátio (local físico) para alocação de motos.
    /// </summary>
    public class PatioDto
    {
        /// <summary>
        /// Identificador único do pátio.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// CEP do pátio.
        /// </summary>
        public string Cep { get; set; } = string.Empty;
        /// <summary>
        /// Número/endereço do pátio.
        /// </summary>
        public string Numero { get; set; } = string.Empty;
        /// <summary>
        /// Cidade.
        /// </summary>
        public string Cidade { get; set; } = string.Empty;
        /// <summary>
        /// Estado (UF).
        /// </summary>
        public string Estado { get; set; } = string.Empty;
        /// <summary>
        /// Capacidade máxima de motos.
        /// </summary>
        public int Capacidade { get; set; }

        public ICollection<MotoDto>? Motos { get; set; }
        public ICollection<AntenaDto>? Antenas { get; set; }
    }

    /// <summary>
    /// Payload para criação de pátio.
    /// </summary>
    public class CreatePatioDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }

    /// <summary>
    /// Payload para atualização de pátio.
    /// </summary>
    public class UpdatePatioDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }
}