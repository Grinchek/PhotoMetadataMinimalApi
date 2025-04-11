using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace PhotoMetadataMinimalApi.Services;

public class MetadataExtractorService
{
    public (DateTime? dateTaken, string? cameraModel, string? gps) Extract(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var directories = ImageMetadataReader.ReadMetadata(stream);

        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        var exifIfd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

        var dateTaken = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
        dateTaken = DateTime.SpecifyKind((DateTime)dateTaken, DateTimeKind.Utc);
        var cameraModel = exifIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel);

        var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();
        var location = gpsDirectory?.GetGeoLocation();
        var gps = location != null
            ? $"{location.Latitude.ToString(CultureInfo.InvariantCulture)}, {location.Longitude.ToString(CultureInfo.InvariantCulture)}"
            : null;

        return (dateTaken, cameraModel, gps);
    }
}
