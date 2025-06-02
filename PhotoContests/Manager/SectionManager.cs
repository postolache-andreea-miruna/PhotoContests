using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using PhotoContests.Repo;

namespace PhotoContests.Manager
{
    public class SectionManager:ISectionManager
    {
        private readonly ISectionRepo sectionRepo;
        private readonly IParticipationRepo participationRepo;
        private readonly UserManager<User> userManager;
        public SectionManager(ISectionRepo sectionRepo, IParticipationRepo participationRepo, UserManager<User> userManager)
        {
            this.sectionRepo = sectionRepo;
            this.participationRepo = participationRepo;
            this.userManager = userManager;
        }

        public void Create(SectionCreateModel model)
        {
            var newSection = new Section
            {
                name = model.name,
                detail = model.detail,
                minimumAge = model.minimumAge,
                presentationUrl = model.presentationUrl,
                presentationYTCode = model.presentationYTCode
            };
            sectionRepo.Create(newSection);
        }

        public void Update(SectionUpdateModel model)
        {
            var section = sectionRepo.GetSectionsIQueryable()
                .FirstOrDefault(s => s.idSection == model.idSection);
            if (section == null) return;
            section.name= model.name;
            section.detail= model.detail;
            section.minimumAge= model.minimumAge;
            section.presentationUrl= model.presentationUrl;
            section.presentationYTCode= model.presentationYTCode;
            sectionRepo.Update(section);
        }

        public void Delete(int id)
        {
            var section = sectionRepo.GetSectionsIQueryable()
                .FirstOrDefault(s => s.idSection == id);
            if (section == null) return;
            sectionRepo.Delete(section);
        }

        public List<SectionUpdateModel?> GetAllSectionsWithId()
        {
            var sections = sectionRepo.GetSectionsIQueryable();
            if (sections == null)
            {
                return new List<SectionUpdateModel>();
            }
            var models = sections.Select(s => new SectionUpdateModel
            {
                idSection = s.idSection,
                name = s.name,
                detail = s.detail,
                minimumAge = s.minimumAge,
                presentationUrl = s.presentationUrl,
                presentationYTCode= s.presentationYTCode
            }).OrderBy(s => s.idSection).ToList();
            return models;
        }

        public List<GetSectionsModel?> GetAllSections()
        {
            var sections = sectionRepo.GetSectionsIQueryable();
            if(sections == null)
            {
                return new List<GetSectionsModel>();
            }
            var models = sections.Select(s => new GetSectionsModel
                                                {
                                                    idSection = s.idSection,
                                                    name = s.name
                                                }).OrderBy(s => s.name).ToList();
            return models;
        }

        public List<SectionCreateModel> GetSectionById(int id)
        {
            var section = sectionRepo.GetSectionsIQueryable()
                .Where(s => s.idSection == id)
                .Select(s => new SectionCreateModel
                {
                    name = s.name,
                    detail = s.detail,
                    minimumAge = s.minimumAge,
                    presentationUrl = s.presentationUrl,
                    presentationYTCode = s.presentationYTCode
                })
                .ToList();
            return section;
        }

        public List<SectionNameModel> GetSectionsCompetitionsByPhotographer(string email, string year_)
        {
            var currentDate = DateTime.Now;
            var users = userManager.Users;
            var idPhotographer = users
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (idPhotographer == null) { return new List<SectionNameModel>(); }
            if (year_ != "allYears")
            {
                var sections = participationRepo.GetParticipationsSectionCompetition()
                .Where(p => p.Photo.Photographer.Id == idPhotographer && p.Competition.startDate.Year.ToString() == year_ && p.Competition.resultsDate <= currentDate)
                .Select(p => new SectionNameModel
                {
                    section = p.Section.name
                })
                .Distinct()
                .OrderBy(p => p.section)
                .ToList();
                return sections;
            }
            else
            {
                var sections = participationRepo.GetParticipationsSectionCompetition()
               .Where(p => p.Photo.Photographer.Id == idPhotographer
                && p.Competition.resultsDate <= currentDate)
               .Select(p => new SectionNameModel
               {
                   section = p.Section.name
               })
               .Distinct()
               .OrderBy(p => p.section)
               .ToList();
                return sections;
            }
            
        }
    }
}
