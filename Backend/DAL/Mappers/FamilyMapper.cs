using Domain;
using Models;
using Riok.Mapperly.Abstractions;

namespace DAL.Mappers
{
    [Mapper]
    public static partial class FamilyMapper
    {
        public static partial FamilyDto ToDto(Family family);
        public static partial Family ToModel(FamilyDto familyDto);
    }
}