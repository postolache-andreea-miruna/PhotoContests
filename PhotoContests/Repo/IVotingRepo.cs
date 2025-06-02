using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public interface IVotingRepo
    {
        void Create(Voting voting);
        void Update(Voting voting);
        void Delete(Voting voting);
        void DeleteVotes(List<Voting> votes);
        IQueryable<Voting> GetVotingIQueryable();
    }
}
