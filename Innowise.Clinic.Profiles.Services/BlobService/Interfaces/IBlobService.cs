using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Services.BlobService.Interfaces;

public interface IBlobService
{
    Task<string?> UploadPhotoAsync(IFormFile? file);
    Task UpdatePhotoAsync(IFormFile file, string savedFileUrl);
    Task DeletePhotoAsync(string photoUrl);
}