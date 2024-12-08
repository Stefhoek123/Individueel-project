using DAL.Containers;
using Domain;
using Interface;
using Models;
using Moq;

namespace TestProject;

[TestClass]
public class FamilyUnitTest
{

    private Mock<IFamilyRepository> _mockFamilyRepository;
    private FamilyContainer _familyContainer;

    [TestInitialize]
    public void SetUp()
    {
        _mockFamilyRepository = new Mock<IFamilyRepository>();
        _familyContainer = new FamilyContainer(_mockFamilyRepository.Object);
    }

    [TestMethod]
    public void GetAllFamilies_ShouldReturnFamilyDtos()
    {
        // Arrange
        var families = new List<Family>
        {
            new Family(Guid.NewGuid(), "Family 1"),
            new Family(Guid.NewGuid(), "Family 2")
        };

        var expectedDtos = families.Select(f => new FamilyDto
        {
            Id = f.Id,
            FamilyName = f.FamilyName,
        }).ToList();

        _mockFamilyRepository.Setup(repo => repo.GetAllFamilies()).Returns(families);

        // Act
        var result = _familyContainer.GetAllFamilies().ToList();

        // Assert
        Assert.AreEqual(expectedDtos.Count, result.Count);
        for (int i = 0; i < expectedDtos.Count; i++)
        {
            Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
            Assert.AreEqual(expectedDtos[i].FamilyName, result[i].FamilyName);
        }
    }


    [TestMethod]
    public void GetFamilyById_ShouldReturnFamilyDto()
    {
        // Arrange
        var familyId = Guid.NewGuid();
        var family = new Family(familyId, "Family 1");

        var expectedDto = new FamilyDto
        {
            Id = family.Id,
            FamilyName = family.FamilyName,
        };

        _mockFamilyRepository.Setup(repo => repo.GetFamilyById(familyId)).Returns(family);

        // Act
        var result = _familyContainer.GetFamilyById(familyId);

        // Assert
        _mockFamilyRepository.Verify(repo => repo.GetFamilyById(familyId), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedDto.Id, result.Id);
        Assert.AreEqual(expectedDto.FamilyName, result.FamilyName);
    }

    [TestMethod]
    public void SearchFamilyByName_ShouldReturnFamilyDtos_WhenSearchIsNotEmpty()
    {
        // Arrange
        var search = "Family 1";
        var families = new List<Family>
        {
            new Family(Guid.NewGuid(), "Family 1"),
            new Family(Guid.NewGuid(), "Family 2")
        };

        var expectedDtos = families.Where(f => f.FamilyName.Contains(search))
            .Select(f => new FamilyDto
            {
                Id = f.Id,
                FamilyName = f.FamilyName,
            }).ToList();

        _mockFamilyRepository.Setup(repo => repo.SearchFamilyByName(search)).Returns(families.Where(f => f.FamilyName.Contains(search)));

        // Act
        var result = _familyContainer.SearchFamilyByName(search).ToList();

        // Assert
        Assert.AreEqual(expectedDtos.Count, result.Count);
        for (int i = 0; i < expectedDtos.Count; i++)
        {
            Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
            Assert.AreEqual(expectedDtos[i].FamilyName, result[i].FamilyName);
        }
    }

    [TestMethod]
    public void SearchFamilyByName_ShouldReturnAllFamilies_WhenSearchIsEmpty()
    {
        // Arrange
        var families = new List<Family>
        {
            new Family(Guid.NewGuid(), "Family 1"),
            new Family(Guid.NewGuid(), "Family 2")
        };

        var expectedDtos = families.Select(f => new FamilyDto
        {
            Id = f.Id,
            FamilyName = f.FamilyName,
        }).ToList();

        _mockFamilyRepository.Setup(repo => repo.GetAllFamilies()).Returns(families);

        // Act
        var result = _familyContainer.SearchFamilyByName(string.Empty).ToList();

        // Assert
        Assert.AreEqual(expectedDtos.Count, result.Count);
        for (int i = 0; i < expectedDtos.Count; i++)
        {
            Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
            Assert.AreEqual(expectedDtos[i].FamilyName, result[i].FamilyName);
        }
    }

    [TestMethod]
    public void CreateFamily_ShouldCreateFamilyInRepository()
    {
        // Arrange
        var familyDto = new FamilyDto
        {
            FamilyName = "Family 1",
        };

        var familyModel = new Family(Guid.NewGuid(), familyDto.FamilyName);

        _mockFamilyRepository.Setup(repo => repo.CreateFamily(It.IsAny<Family>()));

        // Act
        _familyContainer.CreateFamily(familyDto);

        // Assert
        _mockFamilyRepository.Verify(repo => repo.CreateFamily(It.Is<Family>(f =>
            f.FamilyName == familyModel.FamilyName && f.Id != Guid.Empty)), Times.Once);
    }

    [TestMethod]
    public void UpdateFamily_ShouldUpdateFamilyInRepository()
    {
        // Arrange
        var familyDto = new FamilyDto
        {
            Id = Guid.NewGuid(),
            FamilyName = "Updated Family",
        };

        _mockFamilyRepository.Setup(repo => repo.UpdateFamily(It.Is<Family>(f =>
            f.Id == familyDto.Id &&
            f.FamilyName == familyDto.FamilyName 
        )));

        // Act
        _familyContainer.UpdateFamily(familyDto);

        // Assert
        _mockFamilyRepository.Verify(repo => repo.UpdateFamily(It.Is<Family>(f =>
                f.Id == familyDto.Id &&
                f.FamilyName == familyDto.FamilyName
        )), Times.Once);
    }

    [TestMethod]
    public void DeleteFamilyById_ShouldDeleteFamilyInRepository()
    {
        // Arrange
        var familyId = Guid.NewGuid(); 

        _mockFamilyRepository.Setup(repo => repo.DeleteFamilyById(It.Is<Guid>(id => id == familyId)));

        // Act
        _familyContainer.DeleteFamilyById(familyId);

        // Assert
        _mockFamilyRepository.Verify(repo => repo.DeleteFamilyById(familyId), Times.Once, "DeleteFamilyById was not called with the expected ID.");
    }

}   