using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IEmailService
    {
        bool SendEmail(DetailsEmail details);
    }
}
