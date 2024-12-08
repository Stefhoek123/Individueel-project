using DAL.Containers;
using Domain;
using Interface;
using Models;
using Moq;

namespace TestProject;

[TestClass]
public class TextPostUnitTest
{

        private Mock<ITextPostRepository>? _mockTextPostRepository;
        private TextPostContainer? _textPostContainer;

        [TestInitialize]
        public void SetUp()
        {
            _mockTextPostRepository = new Mock<ITextPostRepository>();
            _textPostContainer = new TextPostContainer(_mockTextPostRepository.Object);
        }

        [TestMethod]
        public void GetAllTextPosts_ShouldReturnTextPostDtos()
        {
            // Arrange
            var textPosts = new List<TextPost>
            {
                new TextPost(Guid.NewGuid(), "Title 1", Guid.NewGuid()),
                new TextPost(Guid.NewGuid(), "Title 2", Guid.NewGuid())
            };

            // Manually map the TextPost objects to TextPostDto
            var expectedDtos = textPosts.Select(post => new TextPostDto
            {
                Id = post.Id,
                TextContent = post.TextContent,
                UserId = post.UserId
            }).ToList();

            // Set up the mock repository to return the list of TextPost objects
            _mockTextPostRepository?.Setup(repo => repo.GetAllTextPosts()).Returns(textPosts);

            // Act
            var result = _textPostContainer?.GetAllTextPosts().ToList();

            // Assert
            Assert.AreEqual(expectedDtos.Count, result?.Count);
            Assert.AreEqual(expectedDtos[0].Id, result?[0].Id);
            Assert.AreEqual(expectedDtos[0].TextContent, result?[0].TextContent);
            Assert.AreEqual(expectedDtos[0].UserId, result?[0].UserId);
        }

        [TestMethod]
        public void GetTextPostById_ShouldReturnTextPostDto()
        {
            // Arrange
            var textPostId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var textPost = new TextPost(textPostId, "Sample Title", userId);

            var expectedDto = new TextPostDto
            {
                Id = textPost.Id,
                TextContent = textPost.TextContent,
                UserId = textPost.UserId
            };

            _mockTextPostRepository?.Setup(repo => repo.GetTextPostById(textPostId)).Returns(textPost);

            // Act
            var result = _textPostContainer?.GetTextPostById(textPostId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.TextContent, result.TextContent);
            Assert.AreEqual(expectedDto.UserId, result.UserId);
        }

        [TestMethod]
        public void CreateTextPost_ShouldCreateTextPost()
        {
            // Arrange
            var textPostDto = new TextPostDto
            {
                TextContent = "Sample Title",
                UserId = Guid.NewGuid()
            };

            var newGuid = Guid.NewGuid();

            _mockTextPostRepository?.Setup(repo => repo.CreateTextPost(It.IsAny<TextPost>()));

            // Act
            _textPostContainer?.CreateTextPost(textPostDto);

            // Assert
            _mockTextPostRepository?.Verify(repo => repo.CreateTextPost(It.Is<TextPost>(tp =>
                tp.TextContent == textPostDto.TextContent &&
                tp.UserId == textPostDto.UserId &&
                tp.Id != Guid.Empty)), Times.Once);
        }

        [TestMethod]
        public void UpdateTextPost_ShouldUpdateTextPost()
        {
            // Arrange
            var textPostDto = new TextPostDto
            {
                Id = Guid.NewGuid(),
                TextContent = "Updated Title",
                UserId = Guid.NewGuid()
            };

            _mockTextPostRepository?.Setup(repo => repo.UpdateTextPost(It.IsAny<TextPost>()));

            // Act
            _textPostContainer?.UpdateTextPost(textPostDto);

            // Assert
            _mockTextPostRepository?.Verify(repo => repo.UpdateTextPost(It.Is<TextPost>(tp =>
                tp.Id == textPostDto.Id &&
                tp.TextContent == textPostDto.TextContent &&
                tp.UserId == textPostDto.UserId)), Times.Once);
        }

        [TestMethod]
        public void DeleteTextPostById_ShouldDeleteTextPost()
        {
            // Arrange
            var postId = Guid.NewGuid(); 

            // Act
            _textPostContainer?.DeleteTextPostById(postId);

            // Assert
            _mockTextPostRepository?.Verify(repo => repo.DeleteTextPostById(postId), Times.Once);
        }
}