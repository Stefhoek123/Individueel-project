using DAL.Interfaces;
using Domain;
using Interface;

namespace DAL.Containers;

public class FamilyContainer : IFamilyContainer
{
    // Dependency injection of user repository
    private readonly IFamilyRepository _familyRepository;
    public FamilyContainer(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    // GET for getting all users
    public IEnumerable<FamilyDto> GetAllFamilies()
    {
        return _familyRepository.GetAllFamilies()
            .Select(Mappers.FamilyMapper.ToDto);
    }

    // GET for getting a user based on a User ID
    public FamilyDto GetFamilyById(Guid id)
    {
        return Mappers.FamilyMapper.ToDto(_familyRepository.GetFamilyById(id));
    }

    // GET for finding a User based on his Email or Name
    public IEnumerable<FamilyDto> SearchFamilyByName(string search)
    {
        return string.IsNullOrEmpty(search) ? GetAllFamilies() : _familyRepository.SearchFamilyByName(search).Select(Mappers.FamilyMapper.ToDto);
    }

    // POST Creation of user
    public void CreateFamily(FamilyDto family)
    {
        var familyMapper = Mappers.FamilyMapper.ToModel(family);
        familyMapper.Id = Guid.NewGuid();
        _familyRepository.CreateFamily(familyMapper);
    }

    // PUT for updating a user
    public void UpdateFamily(FamilyDto family)
    {
        _familyRepository.UpdateFamily(Mappers.FamilyMapper.ToModel(family));
    }

    // DELETE of a user op basis van user ID 
    public void DeleteFamilyById(Guid id)
    {
        _familyRepository.DeleteFamilyById(id);
    }
}