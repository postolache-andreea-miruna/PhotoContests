namespace PhotoContests.Models
{
    public class PhotoUpdateModel
    {
        public int idPhoto { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool visibility { get; set; }
    }
}
