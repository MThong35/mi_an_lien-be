using System.ComponentModel.DataAnnotations;

namespace thesis_comicverse_webservice_api.Models
{
    public class Releasing
    {
        public int releasingID { get; set; }
        public bool? isApprove { get; set; }
        public DateTime? approveAt { get; set; }
        public int? userID { get; set; }
        public string? status { get; set; }
        public int? licenseID { get; set; }

    }
}
