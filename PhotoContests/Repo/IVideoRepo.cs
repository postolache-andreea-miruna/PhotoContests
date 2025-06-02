using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IVideoRepo
    {
        void Create(Video video);
        void Update(Video video);
        void Delete(Video video);
        IQueryable<Video> GetVideosIQueryable();
    }
}
