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
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;

        public Guid FamilyId { get; set; }

        public UserDto(Guid id, string firstName, string lastName, string email, string plainPassword, Guid familyId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = plainPassword;
            FamilyId = familyId;
        }

        public UserDto() { }

    }
}
