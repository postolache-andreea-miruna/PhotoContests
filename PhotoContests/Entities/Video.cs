namespace PhotoContests.Entities
{
    public class Video
    {
        /*        public string idPhotographer { get; set; }
                public string idJuror { get; set; }
                public DateTime date { get; set; }

                public string videoUrl { get; set; }
                public string videoYTCode { get; set; }
                public string name { get; set; }

                public Photographer Photographer { get; set; }
                public Juror Juror { get; set; }*/
        public int idVideo { get; set; }
        public string JurorId { get; set; }
        public DateTime date { get; set; }
        public string videoUrl { get; set; }
        public string videoYTCode { get; set; }

        public int idPhoto { get; set; }
        public int idCompetition { get; set; }
        public int idSection { get; set; }

        public Participation Participation { get; set; }
        public Juror Juror { get; set; }
    }
}
