namespace PhotoContests.Models
{
    public class SectionUpdateModel
    {
        public int idSection { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int minimumAge { get; set; }
        public string presentationUrl { get; set; }
        public string presentationYTCode { get; set; }
    }
}
