﻿using DAL.Containers;
using Domain;
using Interface;
using Models;
using Moq;

namespace TestProject;

[TestClass]
public class PostUnitTest
{

        private Mock<IPostRepository>? _mockPostRepository;
        private PostContainer? _postContainer;

        [TestInitialize]
        public void SetUp()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _postContainer = new PostContainer(_mockPostRepository.Object);
        }

        [TestMethod]
        public void GetAllPosts_ShouldReturnPostDtos()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post(Guid.NewGuid(), "Title 1", "url", DateTime.Now, Guid.NewGuid(), Guid.NewGuid()),
                new Post(Guid.NewGuid(), "Title 2", "url", DateTime.Now, Guid.NewGuid(), Guid.NewGuid())
            };

            // Manually map the TextPost objects to TextPostDto
            var expectedDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                TextContent = post.TextContent,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt, 
                UserId = post.UserId,
                FamilyId = post.FamilyId
            }).ToList();

            // Set up the mock repository to return the list of TextPost objects
            _mockPostRepository?.Setup(repo => repo.GetAllPosts()).Returns(posts);

            // Act
            var result = _postContainer?.GetAllPosts().ToList();

            // Assert
            Assert.AreEqual(expectedDtos.Count, result?.Count);
            Assert.AreEqual(expectedDtos[0].Id, result?[0].Id);
            Assert.AreEqual(expectedDtos[0].TextContent, result?[0].TextContent);
            Assert.AreEqual(expectedDtos[0].ImageUrl, result?[0].ImageUrl);
            Assert.AreEqual(expectedDtos[0].CreatedAt, result?[0].CreatedAt);
            Assert.AreEqual(expectedDtos[0].UserId, result?[0].UserId);
            Assert.AreEqual(expectedDtos[0].FamilyId, result?[0].FamilyId);
        }

        [TestMethod]
        public void GetAllPosts_ShouldReturnEmptyList_WhenNoPostsExist()
        {
            // Arrange
            _mockPostRepository?.Setup(repo => repo.GetAllPosts()).Returns(new List<Post>());

            // Act
            var result = _postContainer?.GetAllPosts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }


    [TestMethod]
        public void GetPostById_ShouldReturnPostDto()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
        var familyId = Guid.NewGuid();
        var post = new Post(postId, "Sample Title", "url", DateTime.Now, userId, familyId);

            var expectedDto = new PostDto
            {
                Id = post.Id,
                TextContent = post.TextContent,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId,
                FamilyId = post.FamilyId
            };

            _mockPostRepository?.Setup(repo => repo.GetPostById(postId)).Returns(post);

            // Act
            var result = _postContainer?.GetPostById(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto.Id, result.Id);
            Assert.AreEqual(expectedDto.TextContent, result.TextContent);
        Assert.AreEqual(expectedDto.ImageUrl, result.ImageUrl);
        Assert.AreEqual(expectedDto.CreatedAt, result.CreatedAt);
        Assert.AreEqual(expectedDto.UserId, result.UserId);
        Assert.AreEqual(expectedDto.FamilyId, result.FamilyId);
    }

        [TestMethod]
        public void CreatePost_ShouldCreatePost()
        {
            // Arrange
            var postDto = new PostDto
            {
                TextContent = "Sample Title",
                ImageUrl = "url",
                CreatedAt = DateTime.Now,
                UserId = Guid.NewGuid(),
                FamilyId = Guid.NewGuid()
            };

            var newGuid = Guid.NewGuid();

            _mockPostRepository?.Setup(repo => repo.CreatePost(It.IsAny<Post>()));

            // Act
            _postContainer?.CreatePost(postDto);

            // Assert
            _mockPostRepository?.Verify(repo => repo.CreatePost(It.Is<Post>(tp =>
                tp.TextContent == postDto.TextContent &&
                tp.ImageUrl == postDto.ImageUrl &&
                tp.CreatedAt == postDto.CreatedAt &&
                tp.UserId == postDto.UserId &&
                tp.FamilyId == postDto.FamilyId &&
                tp.Id != Guid.Empty)), Times.Once);
        }

        [TestMethod]
        public void UpdatePost_ShouldUpdatePost()
        {
            // Arrange
            var postDto = new PostDto
            {
                Id = Guid.NewGuid(),
                TextContent = "Updated Title",
                ImageUrl = "url",
                CreatedAt = DateTime.Now,
                UserId = Guid.NewGuid(),
                FamilyId = Guid.NewGuid()
            };

            _mockPostRepository?.Setup(repo => repo.UpdatePost(It.IsAny<Post>()));

            // Act
            _postContainer?.UpdatePost(postDto);

            // Assert
            _mockPostRepository?.Verify(repo => repo.UpdatePost(It.Is<Post>(tp =>
                tp.Id == postDto.Id &&
                tp.TextContent == postDto.TextContent &&
                tp.ImageUrl == postDto.ImageUrl &&
                tp.CreatedAt == postDto.CreatedAt &&
                tp.UserId == postDto.UserId &&
                tp.FamilyId == postDto.FamilyId
                )), Times.Once);
        }

        [TestMethod]
        public void DeletePostById_ShouldDeletePost()
        {
            // Arrange
            var postId = Guid.NewGuid(); 

            // Act
            _postContainer?.DeletePostById(postId);

            // Assert
            _mockPostRepository?.Verify(repo => repo.DeletePostById(postId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeletePostById_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockPostRepository?.Setup(repo => repo.DeletePostById(postId)).Throws(new Exception("Repository error"));

            // Act
            _postContainer?.DeletePostById(postId);

            // Assert
            // Exception is expected, no additional asserts are necessary
        }

}