namespace Core.Iottu.Domain.Entities
{
    public class Moto
    {
        public Guid Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string NumeroMotor { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public StatusMoto Status { get; set; } = null!;
        public Guid TagId { get; set; }
        public Tag Tag { get; set; } = null!;
        public Guid PatioId { get; set; }
        public Patio Patio { get; set; } = null!;

        protected Moto() { }

        public Moto(string placa, string modelo, string chassi, string numeroMotor, int statusId, Guid tagId, Guid patioId)
        {
            Id = Guid.NewGuid();
            Placa = placa;
            Modelo = modelo;
            Chassi = chassi;
            NumeroMotor = numeroMotor;
            StatusId = statusId;
            TagId = tagId;
            PatioId = patioId;
        }

        public void Atualizar(string modelo, string chassi, string numeroMotor, int statusId)
        {
            Modelo = modelo;
            Chassi = chassi;
            NumeroMotor = numeroMotor;
            StatusId = statusId;
        }
    }
}
