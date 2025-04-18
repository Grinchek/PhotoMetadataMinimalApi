﻿namespace PhotoMetadataMinimalApi.Models;

public class Photo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string? CameraModel { get; set; }
    public DateTime? DateTaken { get; set; }
    public string? GpsLocation { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; }
}
