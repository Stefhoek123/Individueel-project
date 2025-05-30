using DAL.Containers;
using Domain;
using Interface;
using Moq;
using Models;
using Microsoft.ApplicationInsights;
using DAL.Mappers;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestProject
{
    [TestClass]
    public class UserUnitTest
    {
        private Mock<IUserRepository>? _mockUserRepository;
        private UserContainer? _userContainer;

        [TestInitialize]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userContainer = new UserContainer(_mockUserRepository.Object);
        }

         [TestMethod]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenUserExistsAndPasswordIsCorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "correctPassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new Models.User(Guid.NewGuid(), "John", "Doe", email, "username", Guid.NewGuid(), 1)
            {
                PasswordHash = hashedPassword
            };

            _mockUserRepository?.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userContainer.AuthenticateUserAsync(email, password);

            // Assert
            
            Assert.IsTrue(result);
            _mockUserRepository?.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
        }

        [TestMethod]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "anyPassword";

            _mockUserRepository?.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync((Models.User)null);

            // Act
            var result = await _userContainer?.AuthenticateUserAsync(email, password);

            // Assert
            Assert.IsFalse(result);
            _mockUserRepository?.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
        }

        [TestMethod]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongPassword";
            var correctPassword = "correctPassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            var user = new Models.User(Guid.NewGuid(), "John", "Doe", email, "username", Guid.NewGuid(), 1)
            {
                PasswordHash = hashedPassword
            };

            _mockUserRepository?.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userContainer.AuthenticateUserAsync(email, password);

            // Assert
            Assert.IsFalse(result);
            _mockUserRepository?.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
        }

        [TestMethod]
        public async Task IsAccountAvailableAsync_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "newuser@example.com";

            _mockUserRepository?.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync((Models.User)null);

            // Act
            var result = await _userContainer.IsAccountAvailableAsync(email);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepository?.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
        }

        [TestMethod]
        public async Task IsAccountAvailableAsync_ShouldReturnFalse_WhenUserExists()
        {
            // Arrange
            var email = "existinguser@example.com";

            var user = new Models.User(Guid.NewGuid(), "John", "Doe", email, "username", Guid.NewGuid(), 1);
            _mockUserRepository?.Setup(repo => repo.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userContainer.IsAccountAvailableAsync(email);

            // Assert
            Assert.IsFalse(result);
            _mockUserRepository?.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
        }

        [TestMethod]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var plainPassword = "MySecurePassword123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            // Act
            var result = _userContainer.VerifyPassword(plainPassword, hashedPassword);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var plainPassword = "MySecurePassword123";
            var wrongPassword = "WrongPassword123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            // Act
            var result = _userContainer.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            Assert.IsFalse(result); 
        }

        [TestMethod]
        public void GetAllUsers_ShouldReturnUserDtos()
        {
            // Arrange
            var users = new List<Models.User>
            {
                new Models.User(Guid.NewGuid(), "John", "Doe", "John123", "john@doe.nl", Guid.NewGuid(), 1),
                new Models.User(Guid.NewGuid(), "Jane", "Doe", "Jane123", "jane@doe.nl", Guid.NewGuid(), 1)
            };

            var expectedDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName ?? string.Empty,
                LastName = u.LastName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                PasswordHash = u.PasswordHash ?? string.Empty,
                FamilyId = u.FamilyId
            }).ToList();

            _mockUserRepository?.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = _userContainer?.GetAllUsers().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDtos.Count, result.Count);
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
                Assert.AreEqual(expectedDtos[i].FirstName, result[i].FirstName);
                Assert.AreEqual(expectedDtos[i].LastName, result[i].LastName);
                Assert.AreEqual(expectedDtos[i].PasswordHash, result[i].PasswordHash);
                Assert.AreEqual(expectedDtos[i].Email, result[i].Email);
                Assert.AreEqual(expectedDtos[i].FamilyId, result[i].FamilyId);
            }

            _mockUserRepository?.Verify(repo => repo.GetAllUsers(), Times.Once);
        }

        [TestMethod]
        public void GetUserById_ShouldReturnUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var familyId = Guid.NewGuid();
            var user = new Models.User(userId, "John",
                "Doe",
                "John@doe.nl",
                "John123",
                familyId,
                1);
            var userDto = new UserDto(userId, "Doe",
                "John",
                "John@doe.nl",
                "Doe123",
                familyId,
                1);

            _mockUserRepository?.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userContainer?.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("John", result.FirstName);

            _mockUserRepository?.Verify(repo => repo.GetUserById(userId), Times.Once);
        }

        [TestMethod]
        public void GetUserByFamilyId_ShouldReturnUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var familyId = Guid.NewGuid();
            var user = new Models.User(userId, "John",
                "Doe",
                "John@doe.nl",
                "John123",
                familyId,
                1);
            var userDto = new UserDto(userId, "Doe",
                "John",
                "John@doe.nl",
                "Doe123",
                familyId,
                1);

            _mockUserRepository?.Setup(repo => repo.GetUserByFamilyId(userId)).Returns(user);

            // Act
            var result = _userContainer?.GetUserByFamilyId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(familyId, result.FamilyId);
            Assert.AreEqual("John", result.FirstName);

            _mockUserRepository?.Verify(repo => repo.GetUserByFamilyId(userId), Times.Once);
        }

        [TestMethod]
        public void GetUsersByFamilyId_ShouldReturnListOfUserDtos()
        {
            // Arrange
            var familyId = Guid.NewGuid();
            var users = new List<Models.User>
            {
                new Models.User(Guid.NewGuid(), "John", "Doe", "john@doe.nl", "John123", familyId, 1),
                new Models.User(Guid.NewGuid(), "Jane", "Doe", "jane@doe.nl", "Jane123", familyId, 1)   
            };

            _mockUserRepository?.Setup(repo => repo.GetUsersByFamilyId(familyId)).Returns(users);

            var expectedDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName ?? string.Empty,
                LastName = u.LastName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                PasswordHash = u.PasswordHash ?? string.Empty,
                FamilyId = u.FamilyId
            }).ToList();

            _mockUserRepository?.Setup(repo => repo.GetUsersByFamilyId(familyId)).Returns(users);

            // Act
            var result = _userContainer?.GetUsersByFamilyId(familyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDtos.Count, result.Count);
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
                Assert.AreEqual(expectedDtos[i].FirstName, result[i].FirstName);
                Assert.AreEqual(expectedDtos[i].LastName, result[i].LastName);
                Assert.AreEqual(expectedDtos[i].PasswordHash, result[i].PasswordHash);
                Assert.AreEqual(expectedDtos[i].Email, result[i].Email);
                Assert.AreEqual(expectedDtos[i].FamilyId, result[i].FamilyId);
            }

            _mockUserRepository?.Verify(repo => repo.GetUsersByFamilyId(familyId), Times.Once);
        }

       

        [TestMethod]
        public void SearchUserByEmailOrName_ShouldReturnAllUsers_WhenSearchIsNullOrEmpty()
        {
            // Arrange
            var users = new List<Models.User>
    {
        new Models.User(Guid.NewGuid(), "John", "Doe", "john@doe.nl", "John123", Guid.NewGuid(), 1),
        new Models.User(Guid.NewGuid(), "Jane", "Smith", "jane@smith.nl", "Jane123", Guid.NewGuid(), 1)
    };

            var expectedDtos = users.Select(u => new UserDto { Id = u.Id, FirstName = u.FirstName }).ToList();

            _mockUserRepository?.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = _userContainer?.SearchUserByEmailOrName(string.Empty).ToList();

            // Assert
            Assert.AreEqual(expectedDtos.Count, result?.Count);
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                Assert.AreEqual(expectedDtos[i].Id, result?[i].Id);
                Assert.AreEqual(expectedDtos[i].FirstName, result?[i].FirstName);
            }

            _mockUserRepository?.Verify(repo => repo.GetAllUsers(), Times.Once);
            _mockUserRepository?.Verify(repo => repo.SearchUserByEmailOrName(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void SearchUserByEmailOrName_ShouldReturnFilteredUsers_WhenSearchIsProvided()
        {
            // Arrange
            var search = "John";
            var users = new List<Models.User>
    {
        new Models.User(Guid.NewGuid(), "John", "Doe", "john@doe.nl", "John123", Guid.NewGuid(), 1)
    };

            var expectedDtos = users.Select(u => new UserDto { Id = u.Id, FirstName = u.FirstName ?? string.Empty }).ToList();

            _mockUserRepository?.Setup(repo => repo.SearchUserByEmailOrName(search)).Returns(users);

            // Act
            var result = _userContainer?.SearchUserByEmailOrName(search).ToList();

            // Assert
            Assert.AreEqual(expectedDtos.Count, result?.Count);
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                Assert.AreEqual(expectedDtos[i].Id, result?[i].Id);
                Assert.AreEqual(expectedDtos[i].FirstName, result?[i].FirstName);
            }

            _mockUserRepository?.Verify(repo => repo.SearchUserByEmailOrName(search), Times.Once);
            _mockUserRepository?.Verify(repo => repo.GetAllUsers(), Times.Never);
        }

        [TestMethod]
        public void CreateUser_ShouldCallCreateUserInRepository()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password",
                FamilyId = Guid.NewGuid(),
                IsActive = 1
            };

            var user = new Models.User(
                userDto.Id,
                userDto.FirstName,
                userDto.LastName,
                userDto.Email,
                userDto.PasswordHash,
                userDto.FamilyId,
                userDto.IsActive
            );

            // Act
            _userContainer?.CreateUser(userDto);

            // Assert
            _mockUserRepository?.Verify(repo => repo.CreateUser(It.Is<Models.User>(u => u.Email == userDto.Email && u.FirstName == userDto.FirstName)), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_ShouldCallUpdateUserInRepository()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password",
                FamilyId = Guid.NewGuid(),
                IsActive = 1
            };

            var user = new Models.User(
                userDto.Id,
                userDto.FirstName,
                userDto.LastName,
                userDto.Email,
                userDto.PasswordHash,
                userDto.FamilyId,
                userDto.IsActive
            );

            // Act
            _userContainer?.UpdateUser(userDto);

            // Assert
            _mockUserRepository?.Verify(repo => repo.UpdateUser(It.Is<Models.User>(u => u.Id == userDto.Id && u.Email == userDto.Email)), Times.Once);
        }

        [TestMethod]
        public void UpdateUserById_ShouldUpdateUserAndCallRepositoryUpdate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password",
                FamilyId = Guid.NewGuid()  ,
                IsActive = 1
            };

            var existingUser = new Models.User(
                userId,
                "Jane", 
                "Doe",
                "jane.doe@example.com",
                "hashed_password",
                Guid.NewGuid(),
                1
            );

            var expectedUser = new Models.User(
                userDto.Id,
                existingUser.FirstName ?? string.Empty, 
                existingUser.LastName ?? string.Empty,
                existingUser.Email ?? string.Empty,
                existingUser.PasswordHash ?? string.Empty,
                userDto.FamilyId,
                userDto.IsActive
            );

            _mockUserRepository?.Setup(repo => repo.GetUserById(userId)).Returns(existingUser);

            // Act
            _userContainer?.UpdateUserById(userId, userDto);

            // Assert
            _mockUserRepository?.Verify(repo => repo.UpdateUser(It.Is<Models.User>(u =>
                u.Id == userDto.Id &&
                u.FamilyId == userDto.FamilyId && 
                u.FirstName == existingUser.FirstName && 
                u.LastName == existingUser.LastName &&
                u.Email == existingUser.Email)), Times.Once);
        }

        [TestMethod]
        public void UpdateUserById_ShouldUpdateFamilyId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newFamilyId = Guid.NewGuid();
            var existingUserDto = new UserDto
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password",
                FamilyId = Guid.NewGuid(),
                IsActive = 1
            };

            var updatedUserDto = new UserDto
            {
                FamilyId = newFamilyId
            };

            _mockUserRepository?.Setup(repo => repo.GetUserById(userId))
                .Returns(new Models.User(
                    existingUserDto.Id,
                    existingUserDto.FirstName,
                    existingUserDto.LastName,
                    existingUserDto.Email,
                    existingUserDto.PasswordHash,
                    existingUserDto.FamilyId,
                    existingUserDto.IsActive
                ));

            // Act
            _userContainer?.UpdateUserById(userId, updatedUserDto);

            // Assert
            _mockUserRepository?.Verify(repo => repo.UpdateUser(It.Is<Models.User>(u =>
                u.Id == userId &&
                u.FamilyId == newFamilyId
            )), Times.Once);
        }


        [TestMethod]
        public void DeleteUserById_ShouldCallDeleteUserByIdInRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            _userContainer?.DeleteUserById(userId);

            // Assert
            _mockUserRepository?.Verify(repo => repo.DeleteUserById(userId), Times.Once);
        }

        [TestMethod]
        public void DeleteUserByFamilyId_ShouldCallDeleteUserByFamilyIdInRepository()
        {
            // Arrange
            var familyId = Guid.NewGuid();

            // Act
            _userContainer?.DeleteUserByFamilyId(familyId);

            // Assert
            _mockUserRepository?.Verify(repo => repo.DeleteUserByFamilyId(familyId), Times.Once);
        }

    }
}
