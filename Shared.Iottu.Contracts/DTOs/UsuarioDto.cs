namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Representa um usuário do sistema.
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Nome de usuário.
        /// </summary>

        public required string Username { get; set; }
        /// <summary>
        /// Hash da senha do usuário.
        /// </summary>
        public required string PasswordHash { get; set; }
        /// <summary>
        /// Papel/role do usuário (ex: admin, user).
        /// </summary>
        public string? Role { get; set; } = "user";
        /// <summary>
        /// Indica se o usuário está ativo.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Data/hora de criação do usuário (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Payload para criação de um usuário.
    /// </summary>
    public class CreateUsuarioDto
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Role { get; set; } = "user";
    }
}