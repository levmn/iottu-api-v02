using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O CEP deve ter entre 8 e 9 caracteres.")]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        [StringLength(10, ErrorMessage = "O número do endereço deve ter no máximo 10 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado (UF) é obrigatório.")]
        [StringLength(2, ErrorMessage = "O estado deve ter 2 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
        public int Capacidade { get; set; }
    }

    /// <summary>
    /// Payload para atualização de pátio.
    /// </summary>
    public class UpdatePatioDto
    {
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O CEP deve ter entre 8 e 9 caracteres.")]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        [StringLength(10, ErrorMessage = "O número do endereço deve ter no máximo 10 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado (UF) é obrigatório.")]
        [StringLength(2, ErrorMessage = "O estado deve ter 2 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
        public int Capacidade { get; set; }
    }
}