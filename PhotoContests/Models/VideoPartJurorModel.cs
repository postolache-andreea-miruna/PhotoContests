namespace PhotoContests.Models
{
    public class VideoPartJurorModel
    {
        public int idVideo { get; set; }
        public string videoUrl { get; set; }
        public string videoYTCode { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
    }
}
