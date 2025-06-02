using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IUserMyManager
    {
        PhotoEmail GetPhotoForEmail(string emailUser);
    }
}
