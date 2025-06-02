using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class NationalityManager : INationalityManager
    {
        private readonly INationalityRepo nationalityRepo;
        private readonly IParticipationRepo participationRepo;
        private readonly IPhotographerRepo photographerRepo;
        public NationalityManager(INationalityRepo nationalityRepo, IParticipationRepo participationRepo, IPhotographerRepo photographerRepo)
        {
            this.nationalityRepo = nationalityRepo;
            this.participationRepo = participationRepo;
            this.photographerRepo = photographerRepo;
        }

        public void Create(NationalityCreateModel model)
        {
            var newNationality = new Nationality
            {
                name = model.name
            };
            nationalityRepo.Create(newNationality);
        }

        public void Update(NationalityUpdateModel model)
        {
            var nationality = nationalityRepo.GetNationalitiesIQueryable().FirstOrDefault(n => n.idNationality == model.idNationality);
            if (nationality == null) return;
            nationality.name = model.name;
            nationalityRepo.Update(nationality);
        }

        public void Delete(int id)
        {
            var nationality = nationalityRepo.GetNationalitiesIQueryable().FirstOrDefault(n => n.idNationality == id);
            if(nationality == null) return;
            nationalityRepo.Delete(nationality);
        }

        public List<NationalityCreateModel?> GetAllNationalities()
        {
            var nationalities = nationalityRepo.GetNationalitiesIQueryable();
            if (nationalities == null)
            {
                return new List<NationalityCreateModel>();
            }

            var models = nationalities.Select(n => new NationalityCreateModel { name= n.name})
                                      .OrderBy(n => n.name)
                                      .ToList();
            return models;
        }
        
        public List<NationalityUpdateModel> GetAllNationalitiesWithId()
        {
            var nationalities = nationalityRepo.GetNationalitiesIQueryable();
            if (nationalities == null)
            {
                return new List<NationalityUpdateModel>();
            }

            var models = nationalities.Select(n => new NationalityUpdateModel 
                                                { 
                                                   idNationality = n.idNationality,
                                                   name = n.name
                                                 })
                                      .OrderBy(n => n.name)
                                      .ToList();
            return models;
        }

        public List<NationalityUpdateModel> GetNationalitiesForCompSection(int idCompetition, int idSection)
        {
            var nationalitites = participationRepo.GetParticipationsSectionCompetitionPhoto()
                .Where(p => p.idCompetition == idCompetition && p.idSection == idSection)
                .Select(p => new NationalityUpdateModel
                {
                    idNationality = photographerRepo.GetPhotographersIQueryable().Where(ph => ph.Id == p.Photo.idUser).Select(ph => ph.idNationality).FirstOrDefault(),
                    name = photographerRepo.GetPhotographersIQueryable().Where(ph => ph.Id == p.Photo.idUser).Select(ph => ph.Nationality.name).FirstOrDefault(),
                })
                .GroupBy(n => new { n.idNationality, n.name }) 
                .Select(g => g.First())
                .ToList();
            return nationalitites;
        }

    }
  
}
