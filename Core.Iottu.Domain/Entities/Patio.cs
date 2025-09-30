namespace Core.Iottu.Domain.Entities
{
    public class Patio
    {
        public Guid Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }

        public ICollection<Moto>? Motos { get; set; }
        public ICollection<Antena>? Antenas { get; set; }
    }
}
