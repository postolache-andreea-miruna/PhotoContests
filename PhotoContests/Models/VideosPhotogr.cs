namespace PhotoContests.Models
{
    public class VideosPhotogr
    {
        public int idCompetition { get; set; }
        public string competitionName { get; set; }
        public List<SectionsVideoModel> sectionVideosDetails { get; set; }
    }
}
