namespace PhotoMetadataMinimalApi.Dtos;

public class PhotoDto
{
    public string Id { get; set; }
    public string FileName { get; set; } = default!;
    public string? CameraModel { get; set; }
    public DateTime? DateTaken { get; set; }
    public string? GpsLocation { get; set; }
}
