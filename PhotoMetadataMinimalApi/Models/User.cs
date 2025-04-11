namespace PhotoMetadataMinimalApi.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nickname { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;

    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
