using PhotoMetadataMinimalApi.Models;

namespace PhotoMetadataMinimalApi.Dtos;

public static class PhotoExtensions
{
    public static PhotoDto ToDto(this Photo photo) => new()
    {
        Id = photo.Id,
        FileName = photo.FileName,
        CameraModel = photo.CameraModel,
        DateTaken = photo.DateTaken,
        GpsLocation = photo.GpsLocation
    };
}
