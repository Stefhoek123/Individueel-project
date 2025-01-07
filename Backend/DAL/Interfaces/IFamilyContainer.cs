using Domain;

namespace DAL.Interfaces;

public interface IFamilyContainer
{
    IEnumerable<FamilyDto> GetAllFamilies();
    FamilyDto GetFamilyById(Guid id);
    IEnumerable<FamilyDto> SearchFamilyByName(string search);
    void CreateFamily(FamilyDto family);
    void UpdateFamily(FamilyDto family);
    void DeleteFamilyById(Guid id);
}