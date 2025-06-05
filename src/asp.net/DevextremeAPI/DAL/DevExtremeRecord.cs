namespace DevextremeAPI.Controllers
{
    public class DevExtremeRecord
    {
        public int Id { get; set; }
        public int Groupe { get; set; }
        public string ValueAsString { get; set; } = string.Empty;
        public double ValueAsDouble { get; set; }
        public int ValueAsInt { get; set; }
    }
}