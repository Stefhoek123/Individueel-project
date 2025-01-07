using Interface;
using Models;

namespace Repositories;

public class FamilyRepository : IFamilyRepository
{
    private readonly BackendDbContext _backendDbContext;

    public FamilyRepository(BackendDbContext backendDbContext)
    {
        _backendDbContext = backendDbContext;
    }

    public IEnumerable<Family> GetAllFamilies()
    {
        return _backendDbContext.Families.ToList();
    }

    public Family GetFamilyById(Guid id)
    {
        return _backendDbContext.Families.FirstOrDefault(f => f.Id == id)!;
    }

    public IEnumerable<Family> SearchFamilyByName(string search)
    {
        IEnumerable<Family> families = _backendDbContext.Families
            .Where(f => f.FamilyName.Contains(search))
            .ToList();
        return families;
    }

    public void CreateFamily(Family family)
    {
        _backendDbContext.Families.Add(family);
        _backendDbContext.SaveChanges();
    }

    public void UpdateFamily(Family family)
    {
        _backendDbContext.Families.Update(family);
        _backendDbContext.SaveChanges();
    }

    public void DeleteFamilyById(Guid id)
    {
        _backendDbContext.Families.Remove(GetFamilyById(id));
        _backendDbContext.SaveChanges();
    }
}