﻿using DAL.Interfaces;
using Domain;
using Interface;
using Repositories;

namespace DAL.Containers;

public class ChatContainer : IChatContainer
{
    private readonly IChatRepository _chatRepository;

    public ChatContainer(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public IEnumerable<ChatDto> GetAllChats()
    {
        return _chatRepository.GetAllChats()
            .Select(Mappers.ChatMapper.ToDto);
    }

    public ChatDto GetChatById(Guid id)
    {
        return Mappers.ChatMapper.ToDto(_chatRepository.GetChatById(id));
    }

    public List<ChatDto> GetChatsById(Guid id)
    {
        var chats = _chatRepository.GetChatsById(id);

        return chats.Select(c => Mappers.ChatMapper.ToDto(c)).ToList();
    }

    public void CreateChat(ChatDto chat)
    {
        var chatMapper = Mappers.ChatMapper.ToModel(chat);
        chatMapper.Id = Guid.NewGuid();
        _chatRepository.CreateChat(chatMapper);
    }

    public void UpdateChat(ChatDto chat)
    {
        _chatRepository.UpdateChat(Mappers.ChatMapper.ToModel(chat));
    }

    public void DeleteChatById(Guid id)
    {
        _chatRepository.DeleteChatById(id);
    }
}