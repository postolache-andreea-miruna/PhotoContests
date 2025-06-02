using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface IVotingManager
    {
        void Create(VotingCreateModel model);
        bool VotedParticipation(VotedParticipationModel model);
        void Delete(string emailUser, int idPhoto, int idSection, int idCompetition);
        int GetJurorVoteForParticipation(string emailJuror, int idPhoto, int idSection, int idCompetition);
        void Update(VotingCreateModel model);
    }
}
