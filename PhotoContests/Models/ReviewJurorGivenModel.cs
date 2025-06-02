namespace PhotoContests.Models
{
    public class ReviewJurorGivenModel
    {
        public int idReview { get; set; }
        public string idUser { get; set; }
        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }
        public string message { get; set; }
        public DateTime sendDate { get; set; }
        public string emailJuror { get; set; }
    }
}
