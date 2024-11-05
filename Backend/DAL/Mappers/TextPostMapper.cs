using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class TextPostMapper
    {
        public static partial TextPostDto ToDto(TextPost textPost);
        public static partial TextPost ToModel(TextPostDto textPostDto);
    }
}