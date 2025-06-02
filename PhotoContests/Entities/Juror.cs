namespace PhotoContests.Entities
{
    public class Juror: User
    {
        public int yearsOfExperience { get; set; }

        public ICollection<Video> Videos { get; set; }
        public ICollection<Assignement> Assignements { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
