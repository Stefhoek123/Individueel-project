using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public Guid FamilyId { get; init; }
        public FamilyDto? Family { get; set; }

        public UserDto(Guid id, string firstName, string lastName, string email, string password, Guid familyId, FamilyDto? family)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            FamilyId = familyId;
            Family = family;
        }

        public UserDto() { }
    }
}
