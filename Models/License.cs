namespace thesis_comicverse_webservice_api.Models
{
    public class License
    {
        public int licenseID { get; set; }
        public string? licenseName { get; set; }
        public string? licenseType { get; set; }
        public string? provider { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validUntil { get; set; }
    }
}
