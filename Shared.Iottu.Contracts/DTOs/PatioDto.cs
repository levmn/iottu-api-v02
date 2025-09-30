namespace Shared.Iottu.Contracts.DTOs
{
    public class PatioDto
    {
        public Guid Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }

        public ICollection<MotoDto>? Motos { get; set; }
        public ICollection<AntenaDto>? Antenas { get; set; }
    }

    public class CreatePatioDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }

    public class UpdatePatioDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }
}