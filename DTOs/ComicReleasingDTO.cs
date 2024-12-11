namespace thesis_comicverse_webservice_api.DTOs
{
    public class ComicReleasingDTO
    {
        public int ComicId { get; set; }
        public string comicTitle { get; set; }
        public string localhostURL { get; set; }
        public string remoteURL { get; set; }
        public int? taskID { get; set; }
        public DateTime? releaseDate { get; set; }
        public int? publisherID { get; set; }
        public int? releasingID { get; set; }
        public int? authorID { get; set; }
        public string language { get; set; }
        public int? categoryID { get; set; }
        public string Description { get; set; }
        public string avatarURL { get; set; }
        public bool? isApprove { get; set; }
        public DateTime? approveAt { get; set; }
        public int? userID { get; set; }
        public string status { get; set; }
        public int? licenseID { get; set; }
    }
}
