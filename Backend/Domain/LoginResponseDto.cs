namespace Domain;

public class LoginResponseDto
{
    public string Email { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }

}