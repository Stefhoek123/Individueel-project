using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        public static partial UserDto ToDto(User user);
        public static partial User ToModel(UserDto userDto);
    }
}