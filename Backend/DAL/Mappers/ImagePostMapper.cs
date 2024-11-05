using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class ImagePostMapper
    {
        public static partial ImagePostDto ToDto(ImagePost imagePost);
        public static partial ImagePost ToModel(ImagePostDto imagePostDto);
    }
}