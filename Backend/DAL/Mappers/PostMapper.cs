using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class PostMapper
    {
        public static partial PostDto ToDto(Post post);
        public static partial Post ToModel(PostDto postDto);
    }
}