using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class PhotoRepo: IPhotoRepo
    {
        private PhotoContestsContext db;
        public PhotoRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Photo photo)
        {
            db.Photos.Add(photo);
            db.SaveChanges();
        }

        public void Update(Photo photo)
        {
            db.Photos.Update(photo);
            db.SaveChanges();
        }

        public void Delete(Photo photo)
        {
            db.Photos.Remove(photo);
            db.SaveChanges();
        }

        public IQueryable<Photo> GetPhotosIQueryable()
        {
            var photos = db.Photos;
            return photos;
        }
    }
}
