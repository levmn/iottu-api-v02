namespace Core.Iottu.Domain.Entities
{
    public class Antena
    {
        public Guid Id { get; private set; }
        public string Localizacao { get; private set; }
        public string Identificador { get; private set; }

        protected Antena() { }

        public Antena(string localizacao, string identificador)
        {
            Id = Guid.NewGuid();
            Localizacao = localizacao;
            Identificador = identificador;
        }
    }
}
