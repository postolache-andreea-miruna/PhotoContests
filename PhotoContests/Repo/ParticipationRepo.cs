using Microsoft.EntityFrameworkCore;
using PhotoContests.Entities;

namespace PhotoContests.Repo
{
    public class ParticipationRepo: IParticipationRepo
    {
        private PhotoContestsContext db;
        public ParticipationRepo(PhotoContestsContext db)
        {
            this.db = db;
        }

        public void Create(Participation participation)
        {
            db.Participations.Add(participation);
            db.SaveChanges();
        }

        public void Update(Participation participation)
        {
            db.Participations.Update(participation);
            db.SaveChanges();
        }

        public void UpdateRange(IEnumerable<Participation> participations)
        {
            db.Participations.UpdateRange(participations);
            db.SaveChanges();
        }

        public void Delete(Participation participation)
        {
            db.Participations.Remove(participation);
            db.SaveChanges();
        }

        public IQueryable<Participation> GetParticipationsSectionCompetition()
        {
            var participations = db.Participations
                .Include(s => s.Section)
                .Include(c => c.Competition);
            return participations;
        }

        public IQueryable<Participation> GetParticipationsSectionCompetitionPhoto()
        {
            var participations = GetParticipationsSectionCompetition()
                .Include(p => p.Photo);
            return participations;
        }
    }
}
