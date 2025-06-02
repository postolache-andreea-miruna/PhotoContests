using Microsoft.EntityFrameworkCore;
using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class VotingRepo:IVotingRepo
    {
        private PhotoContestsContext db;
        public VotingRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create (Voting voting)
        {
            db.Votings.Add(voting);
            db.SaveChanges();
        }

        public void Update (Voting voting)
        {
            db.Votings.Update(voting);
            db.SaveChanges();
        }

        public void Delete (Voting voting)
        {
            db.Votings.Remove(voting);
            db.SaveChanges();
        }
        public void DeleteVotes(List<Voting> votes)
        {
            db.Votings.RemoveRange(votes);
            db.SaveChanges();
        }

        public IQueryable<Voting> GetVotingIQueryable()
        {
            var vots = db.Votings;
            return vots;
        }
    }
}
