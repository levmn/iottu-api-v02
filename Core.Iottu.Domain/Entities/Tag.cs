namespace Core.Iottu.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; private set; }
        public string CodigoRFID { get; private set; } = null!;
        public string SSIDWifi { get; private set; } = null!;
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DateTime DataHora { get; private set; }

        public Moto? Moto { get; private set; }
        public Guid AntenaId { get; private set; }
        public Antena? Antena { get; private set; }

        protected Tag() { }

        public Tag(string codigoRFID, string ssidWifi, double latitude, double longitude, DateTime dataHora, Guid antenaId)
        {
            Id = Guid.NewGuid();
            CodigoRFID = codigoRFID;
            SSIDWifi = ssidWifi;
            Latitude = latitude;
            Longitude = longitude;
            DataHora = dataHora;
            AntenaId = antenaId;
        }
    }
}
