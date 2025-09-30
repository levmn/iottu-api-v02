namespace Shared.Iottu.Contracts.DTOs
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public required string CodigoRFID { get; set; }
        public required string SSIDWifi { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required DateTime DataHora { get; set; }

        public MotoDto? Moto { get; set; }

        public Guid AntenaId { get; set; }
        public AntenaDto? Antena { get; set; }

    }

    public class CreateTagDto
    {
        public string CodigoRFID { get; set; } = string.Empty;
        public string SSIDWifi { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public Guid AntenaId { get; set; }
    }

    public class UpdateTagDto
    {
        public string CodigoRFID { get; set; } = string.Empty;
        public string SSIDWifi { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public Guid AntenaId { get; set; }
    }
}
