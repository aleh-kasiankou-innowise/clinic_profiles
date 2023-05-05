using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Services.BlobService.Interfaces;

public interface IBlobService
{
    Task<string?> SavePhotoAsync(Guid fileId, IFormFile? file);
    Task DeletePhotoAsync(string photoUrl);
}