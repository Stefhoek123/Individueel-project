using DAL.Containers;
using Domain;
using Interface;
using Moq;
using Models;
using Microsoft.ApplicationInsights;

namespace TestProject
{
    [TestClass]
    public class UserUnitTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserContainer _userContainer;

        [TestInitialize]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userContainer = new UserContainer(_mockUserRepository.Object);

        }

        [TestMethod]
        public void GetAllUsers_ShouldReturnUserDtos()
        {
            Guid myGuid = Guid.NewGuid();

            // Arrange
            var users = new List<User>
            {
                new User(myGuid, "John",  "Doe",  "John123","John@doe.nl", myGuid),
                new User(myGuid, "Doe",  "John",  "Doe123","Doe@john.nl", myGuid)
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(users);

            var expectedDtos = users.Select(user => new UserDto { Id = user.Id, FirstName = user.FirstName }).ToList();
           
            // Act
            var result = _userContainer.GetAllUsers().ToList();

            // Assert
            Assert.AreEqual(expectedDtos.Count, result.Count);
            Assert.AreEqual(expectedDtos[0].Id, result[0].Id);
            Assert.AreEqual(expectedDtos[0].FirstName, result[0].FirstName);
        }

        [TestMethod]
        public void GetUserById_ShouldReturnUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var familyId = Guid.NewGuid();
            var user = new User(userId, "John",
                "Doe",
                "John@doe.nl",
                "123",
                familyId);
            var userDto = new UserDto(userId, "John",
                "Doe",
                "John@doe.nl",
                "123",
                familyId);


            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userContainer.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("John", result.FirstName);

        }
    }
}
