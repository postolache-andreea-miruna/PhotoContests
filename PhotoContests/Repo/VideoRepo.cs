using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class VideoRepo:IVideoRepo
    {
        private PhotoContestsContext db;
        public VideoRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Video video)
        {
            db.Videos.Add(video);
            db.SaveChanges();
        }

        public void Update(Video video)
        {
            db.Videos.Update(video);
            db.SaveChanges();
        }
        public void Delete(Video video)
        {
            db.Videos.Remove(video);
            db.SaveChanges();
        }

        public IQueryable<Video> GetVideosIQueryable()
        {
            var videos = db.Videos;
            return videos;
        }
    }

}
