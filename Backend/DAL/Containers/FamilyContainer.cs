using DAL.Interfaces;
using Domain;
using Interface;

namespace DAL.Containers;

public class FamilyContainer : IFamilyContainer
{
    // Dependency injection of family repository
    private readonly IFamilyContainer _familyRepository;
    public FamilyContainer(IFamilyContainer familyRepository)
    {
        _familyRepository = familyRepository;
    }

    // GET for getting all families
    public IEnumerable<FamilyDto> GetAllFamilies()
    {
        return _familyRepository.GetAllFamilies()
            .Select(Mappers.FamilyMapper.ToDto);
    }

    // GET for getting a family based on a family ID
    public FamilyDto GetFamilyById(Guid id)
    {
        return Mappers.FamilyMapper.ToDto(_familyRepository.GetFamilyById(id));
    }

    // GET for finding a family based on Name
    public IEnumerable<FamilyDto> SearchFamilyByName(string search)
    {
        return string.IsNullOrEmpty(search) ? GetAllFamilies() : _familyRepository.SearchFamilyByName(search).Select(Mappers.FamilyMapper.ToDto);
    }

    // POST Creation of family
    public void CreateFamily(FamilyDto familydto)
    {
        var family = Mappers.FamilyMapper.ToModel(familydto);
        family.Id = Guid.NewGuid();
        _familyRepository.CreateFamily(family);
    }

    // PUT for updating a family
    public void UpdateFamily(FamilyDto family)
    {
        _familyRepository.UpdateFamily(Mappers.FamilyMapper.ToModel(family));
    }

    // DELETE of a family op basis van user ID 
    public void DeleteFamilyById(Guid id)
    {
        _familyRepository.DeleteFamilyById(id);
    }
}