namespace Core.Iottu.Domain.Entities
{
    public class Moto
    {
        public Guid Id { get; private set; }
        public string Placa { get; private set; }
        public string Modelo { get; private set; }
        public string Cor { get; private set; }

        public Guid TagId { get; private set; }
        public Tag? Tag { get; private set; }

        protected Moto() { }

        public Moto(string placa, string modelo, string cor, Guid tagId)
        {
            Id = Guid.NewGuid();
            Placa = placa;
            Modelo = modelo;
            Cor = cor;
            TagId = tagId;
        }

        public void Atualizar(string modelo, string cor)
        {
            Modelo = modelo;
            Cor = cor;
        }
    }
}
