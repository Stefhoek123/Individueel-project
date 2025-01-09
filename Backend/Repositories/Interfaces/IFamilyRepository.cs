using Models;
using Repositories;

namespace Interface;

public interface IFamilyRepository
{
    IEnumerable<Family> GetAllFamilies();
    Family GetFamilyById(Guid id);
    Family GetFamilyIdByName(string familyName);
    IEnumerable<Family> SearchFamilyByName(string search);
    void CreateFamily(Family family);
    void UpdateFamily(Family family);
    void DeleteFamilyById(Guid id);
}