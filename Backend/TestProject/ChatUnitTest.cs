using DAL.Containers;
using DAL.Interfaces;
using Domain;
using Interface;
using Models;
using Moq;
using System;

namespace TestProject;

[TestClass]
public class ChatUnitTest
{
    private Mock<IChatRepository>? _mockChatRepository;
    private ChatContainer? _chatContainer;

    [TestInitialize]
    public void SetUp()
    {
        _mockChatRepository = new Mock<IChatRepository>();
        _chatContainer = new ChatContainer(_mockChatRepository.Object);
    }

    [TestMethod]
    public void GetAllChats_ShouldReturnChatDtos()
    {
        // Arrange
        var chats = new List<Chat>
        {
            new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "test content 1", Guid.NewGuid(), "Test sender 1", Guid.NewGuid()),
            new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "test content 2", Guid.NewGuid(), "Test sender 2", Guid.NewGuid())
        };

        var expectedDtos = chats.Select(c => new ChatDto
        {
            Id = c.Id,
            PostId = c.PostId,
            Date = c.Date,
            ChatContent = c.ChatContent,
            ReactId = c.ReactId,
            SenderName = c.SenderName,
            UserId = c.UserId

        }).ToList();

        _mockChatRepository?.Setup(repo => repo.GetAllChats()).Returns(chats);

        // Act
        var result = _chatContainer?.GetAllChats().ToList();

        // Assert
        Assert.AreEqual(expectedDtos.Count, result.Count);
        for (int i = 0; i < expectedDtos.Count; i++)
        {
            Assert.AreEqual(expectedDtos[i].Id, result[i].Id);
            Assert.AreEqual(expectedDtos[i].PostId, result[i].PostId);
            Assert.AreEqual(expectedDtos[i].Date, result[i].Date);
            Assert.AreEqual(expectedDtos[i].ChatContent, result[i].ChatContent);
            Assert.AreEqual(expectedDtos[i].ReactId, result[i].ReactId);
            Assert.AreEqual(expectedDtos[i].SenderName, result[i].SenderName);
            Assert.AreEqual(expectedDtos[i].UserId, result[i].UserId);
        }
    }

    [TestMethod]
    public void GetChatById_ShouldReturnChatDto()
    {
        // Arrange
        var chatId = Guid.NewGuid();
        var chat = new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "test content 2", Guid.NewGuid(),
            "Test sender 2", Guid.NewGuid());

        var expectedDto = new ChatDto
        {
            Id = chat.Id,
            PostId = chat.PostId,
            Date = chat.Date,
            ChatContent = chat.ChatContent,
            ReactId = chat.ReactId,
            SenderName = chat.SenderName,
            UserId = chat.UserId
        };

        _mockChatRepository?.Setup(repo => repo.GetChatById(chatId)).Returns(chat);

        // Act
        var result = _chatContainer?.GetChatById(chatId);

        // Assert
        _mockChatRepository?.Verify(repo => repo.GetChatById(chatId), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedDto.Id, result.Id);
        Assert.AreEqual(expectedDto.PostId, result.PostId);
        Assert.AreEqual(expectedDto.Date, result.Date);
        Assert.AreEqual(expectedDto.ChatContent, result.ChatContent);
        Assert.AreEqual(expectedDto.ReactId, result.ReactId);
        Assert.AreEqual(expectedDto.SenderName, result.SenderName);
        Assert.AreEqual(expectedDto.UserId, result.UserId);
    }

    [TestMethod]
    public void GetChatsById_ShouldReturnChatDtos()
    {
        // Arrange
        var chatId = Guid.NewGuid();
        var chats = new List<Chat>
        {
            new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "test content 1", Guid.NewGuid(), "Test sender 1", Guid.NewGuid()),
            new Chat(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, "test content 2", Guid.NewGuid(), "Test sender 2", Guid.NewGuid())
        };

        var expectedChatDtos = chats.Select(chat => new ChatDto
        {
            Id = chat.Id,
            PostId = chat.PostId,
            Date = chat.Date,
            ChatContent = chat.ChatContent,
            ReactId = chat.ReactId,
            SenderName = chat.SenderName,
            UserId = chat.UserId
        }).ToList();

        _mockChatRepository?.Setup(repo => repo.GetChatsById(chatId)).Returns(chats);

        // Act
        var result = _chatContainer?.GetChatsById(chatId);

        // Assert
        Assert.AreEqual(expectedChatDtos.Count, result.Count);
        for (int i = 0; i < expectedChatDtos.Count; i++)
        {
            Assert.AreEqual(expectedChatDtos[i].Id, result[i].Id);
            Assert.AreEqual(expectedChatDtos[i].PostId, result[i].PostId);
            Assert.AreEqual(expectedChatDtos[i].Date, result[i].Date);
            Assert.AreEqual(expectedChatDtos[i].ChatContent, result[i].ChatContent);
            Assert.AreEqual(expectedChatDtos[i].ReactId, result[i].ReactId);
            Assert.AreEqual(expectedChatDtos[i].SenderName, result[i].SenderName);
            Assert.AreEqual(expectedChatDtos[i].UserId, result[i].UserId);
        }

        _mockChatRepository?.Verify(repo => repo.GetChatsById(chatId), Times.Once);
    }

    [TestMethod]
    public void CreateChat_ShouldCreateChatInRepository()
    {
        // Arrange
        var chatDto = new ChatDto
        {
            PostId = Guid.NewGuid(),
            Date = DateTime.Now,
            ChatContent = "test content 2",
            ReactId = Guid.NewGuid(),
            SenderName = "Test sender 2",
            UserId = Guid.NewGuid()
        };

        var chatModel = new Chat(Guid.NewGuid(), chatDto.PostId, chatDto.Date, chatDto.ChatContent, chatDto.ReactId, chatDto.SenderName, chatDto.UserId);

        _mockChatRepository?.Setup(repo => repo.CreateChat(It.IsAny<Chat>()));

        // Act
        _chatContainer?.CreateChat(chatDto);

        // Assert
        _mockChatRepository?.Verify(repo => repo.CreateChat(It.Is<Chat>(c =>
            c.PostId == chatModel.PostId && c.ChatContent == chatModel.ChatContent && c.Id != Guid.Empty)), Times.Once);
    }

    [TestMethod]
    public void UpdateChat_ShouldUpdateChatInRepository()
    {
        // Arrange
        var chatDto = new ChatDto
        {
            Id = Guid.NewGuid(),
            PostId = Guid.NewGuid(),
            Date = DateTime.Now,
            ChatContent = "test content 2",
            ReactId = Guid.NewGuid(),
            SenderName = "Test sender 2",
            UserId = Guid.NewGuid()
        };

        _mockChatRepository?.Setup(repo => repo.UpdateChat(It.Is<Chat>(c =>
            c.Id == chatDto.Id &&
            c.PostId == chatDto.PostId &&
            c.ChatContent == chatDto.ChatContent
        )));

        // Act
        _chatContainer?.UpdateChat(chatDto);

        // Assert
        _mockChatRepository?.Verify(repo => repo.UpdateChat(It.Is<Chat>(c =>
            c.Id == chatDto.Id &&
            c.PostId == chatDto.PostId &&
            c.ChatContent == chatDto.ChatContent
        )), Times.Once);
    }

    [TestMethod]
    public void DeleteChatById_ShouldDeleteChatInRepository()
    {
        // Arrange
        var chatId = Guid.NewGuid();

        _mockChatRepository?.Setup(repo => repo.DeleteChatById(It.Is<Guid>(id => id == chatId)));

        // Act
        _chatContainer?.DeleteChatById(chatId);

        // Assert
        _mockChatRepository?.Verify(repo => repo.DeleteChatById(chatId), Times.Once, "DeleteChatById was not called with the expected ID.");
    }
}