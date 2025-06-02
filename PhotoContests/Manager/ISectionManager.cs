using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public interface ISectionManager
    {
        void Create(SectionCreateModel model);
        void Update(SectionUpdateModel model);
        void Delete(int id);
        List<GetSectionsModel?> GetAllSections();
        List<SectionCreateModel> GetSectionById(int id);
        List<SectionUpdateModel?> GetAllSectionsWithId();

        List<SectionNameModel> GetSectionsCompetitionsByPhotographer(string email, string comp);
    }
}
