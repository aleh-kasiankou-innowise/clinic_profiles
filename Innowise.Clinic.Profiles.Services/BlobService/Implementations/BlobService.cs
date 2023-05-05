using Innowise.Clinic.Profiles.Services.BlobService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Services.BlobService.Implementations;

public class BlobService : IBlobService
{
    private readonly IRequestClient<BlobSaveRequest> _saveClient;

    private readonly IRequestClient<BlobDeletionRequest> _deleteClient;


    public BlobService(IRequestClient<BlobDeletionRequest> deleteClient, IRequestClient<BlobSaveRequest> saveClient)
    {
        _deleteClient = deleteClient;
        _saveClient = saveClient;
    }

    public async Task<string?> SavePhotoAsync(Guid fileId, IFormFile? file)
    {
        if (file is null)
        {
            return null;
        }

        var fileContent = await ConvertFileToBytes(file);
        var uploadRequest = new BlobSaveRequest(fileId, BlobCategories.ProfilePhoto, fileContent, file.ContentType);
        var response = await _saveClient.GetResponse<BlobSaveResponse>(uploadRequest);
        if (!response.Message.IsSuccessful)
        {
            throw new ApplicationException("The photo cannot be uploaded. The profile creation/update is cancelled.");
        }

        return response.Message.FileUrl;
    }

    public async Task DeletePhotoAsync(string photoUrl)
    {
        var deletionRequest = new BlobDeletionRequest(photoUrl);
        var response = await _deleteClient.GetResponse<BlobDeletionResponse>(deletionRequest);
        if (!response.Message.IsSuccessful)
        {
            throw new ApplicationException("The photo cannot be deleted. The profile deletion is cancelled.");
        }
    }

    private async Task<byte[]> ConvertFileToBytes(IFormFile file)
    {
        using var contentStream = new MemoryStream();
        await file.CopyToAsync(contentStream);
        return contentStream.ToArray();
    }
}