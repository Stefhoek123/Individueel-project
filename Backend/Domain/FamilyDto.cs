using System.ComponentModel.DataAnnotations;

namespace Domain;

public class FamilyDto
{
    public Guid Id { get; set; }

    [Required]
    public string FamilyName { get; set; } = null!;

    public FamilyDto(Guid id, string familyName)
    {
        Id = id;
        FamilyName = familyName;
    }

    public FamilyDto() { }
}
