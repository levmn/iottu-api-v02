namespace Core.Iottu.Domain.Entities
{
    public class StatusMoto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public ICollection<Moto>? Motos { get; set; }
    }
}


