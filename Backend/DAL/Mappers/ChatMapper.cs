using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class ChatMapper
    {
        public static partial ChatDto ToDto(Chat chat);
        public static partial Chat ToModel(ChatDto chatDto);
    }
}