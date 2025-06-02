namespace PhotoContests.Entities
{
    public class Assignement
    {
        public string idUser { get; set; }
        public int idCompetition { get; set; }
        public DateTime assignementDate { get; set; } = DateTime.Now;

        public Juror Juror { get; set; }
        public Competition Competition { get; set; }

    }
}
