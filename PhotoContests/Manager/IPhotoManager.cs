using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IPhotoManager
    {
        /*void Create(PhotoCreateModel model);*/
        int Create(PhotoCreateModel model);
        void Update(PhotoUpdateModel model);
        void Delete(int id);
        List<GetAllPhotosModel> GetAllPhotosByPhotographer(string email);
        List<GetAllPhotosModel> GetAllPhotosByPhotographerVisible(string email);
        GetPhotoModel GetPhotoInfo(int id);

        //new
        List<PortofolioPhotosModel> GetPortofolioPhotographer(string email);
        List<PortofolioPhotosOtherViewModel> GetPortofolioPhotographerOtherView(string email);
        List<GetAllPhotosModel> GetAllPhotosDontPartByPhotographer(string email, int idComp, int idSect);
    }
}
