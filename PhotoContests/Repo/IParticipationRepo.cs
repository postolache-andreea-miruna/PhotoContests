using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IParticipationRepo
    {
        void Create(Participation participation);
        void Update(Participation participation);
        void UpdateRange(IEnumerable<Participation> participations);
        void Delete(Participation participation);
        IQueryable<Participation> GetParticipationsSectionCompetition();
        IQueryable<Participation> GetParticipationsSectionCompetitionPhoto();
    }
}
