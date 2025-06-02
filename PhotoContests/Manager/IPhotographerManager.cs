using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IPhotographerManager
    {
        PhotographerByEmailModel GetPhotographerInfo(string email);
        PhotographerByEmailModel GetPhotographerInfoById(string id);
        void Update(PhotographerUpdateModel model);
    }
}
