using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IVideoManager
    {
        void Create(VideoCreateModel model);
        void Delete(int idVideo);
        void Update(VideoUpdateModel model);
        VideoPartJurorModel GetVideoForPartByJuror(string emailJuror, int idPhoto, int idCompetition, int idSection);
        /*void Delete(string idPhotographer, string idJuror, DateTime date);
        List<VideoModel> GetAllVideosByPhotogrAndName(string email, string name = "all competitions");
        List<VideoJurorMode> GetAllVideosByJurorAndName(string emailJuror, string name = "all competitions");*/

        // List<VideosPhotogr> VideosReceivedByPhotogr(string emailPhotogr);
        List<VideosPhotogr> VideosReceivedByPhotogr(string emailPhotogr, string competitionName);
    }
}
