namespace PhotoMetadataMinimalApi.Dtos;

public class RegisterUserDto
{
    public string Nickname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
}
