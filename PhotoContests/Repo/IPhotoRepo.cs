using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IPhotoRepo
    {
        void Create(Photo photo);
        void Update(Photo photo);
        void Delete(Photo photo);
        IQueryable<Photo> GetPhotosIQueryable();
    }
}
