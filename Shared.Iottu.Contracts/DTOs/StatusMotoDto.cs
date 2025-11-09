namespace Shared.Iottu.Contracts.DTOs
{
    public class StatusMotoDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public ICollection<MotoDto>? Motos { get; set; }
    }
}
