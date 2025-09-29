namespace Core.Iottu.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; private set; }
        public string Codigo { get; private set; }
        public bool Ativa { get; private set; }

        public Moto? Moto { get; private set; }

        protected Tag() { }

        public Tag(string codigo)
        {
            Id = Guid.NewGuid();
            Codigo = codigo;
            Ativa = true;
        }

        public void Desativar() => Ativa = false;
        public void Ativar() => Ativa = true;
    }
}
