using PhotoContests.Entities;

namespace PhotoContests.Models
{
    public class VideoCreateModel
    {
        // public string emailPhotographer { get; set; }
        //public string emailJuror { get; set; }
        // public string videoUrl { get; set; }
        //  public string videoYTCode { get; set; }
        //  public string name { get; set; }
        public string emailJuror { get; set; }
        public string videoUrl { get; set; }
        public string videoYTCode { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }

    }
}
