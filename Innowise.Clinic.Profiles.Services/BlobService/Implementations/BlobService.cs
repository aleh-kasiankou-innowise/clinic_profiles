using Innowise.Clinic.Profiles.Services.BlobService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Services.BlobService.Implementations;

public class BlobService : IBlobService
{
    private readonly IRequestClient<BlobUploadRequest> _uploadClient;
    private readonly IRequestClient<BlobUpdateRequest> _updateClient;
    private readonly IRequestClient<BlobDeletionRequest> _deleteClient;


    public BlobService(IRequestClient<BlobUploadRequest> uploadClient, IRequestClient<BlobUpdateRequest> updateClient,
        IRequestClient<BlobDeletionRequest> deleteClient)
    {
        _uploadClient = uploadClient;
        _updateClient = updateClient;
        _deleteClient = deleteClient;
    }

    public async Task<string?> UploadPhotoAsync(IFormFile? file)
    {
        if (file is null)
        {
            return null;
        }

        var fileContent = await ConvertFileToBytes(file);
        var uploadRequest = new BlobUploadRequest(fileContent, file.ContentType, BlobCategories.ProfilePhoto);
        var response = await _uploadClient.GetResponse<BlobUploadResponse>(uploadRequest);
        if (!response.Message.IsSuccessful)
        {
            throw new ApplicationException("The photo cannot be uploaded. The profile creation/update is cancelled.");
        }

        return response.Message.FileUrl;
    }

    public async Task UpdatePhotoAsync(IFormFile file, string savedFileUrl)
    {
        var fileContent = await ConvertFileToBytes(file);
        var updateRequest = new BlobUpdateRequest(fileContent, file.ContentType, savedFileUrl);
        var response = await _updateClient.GetResponse<BlobUpdateResponse>(updateRequest);
        if (!response.Message.IsSuccessful)
        {
            throw new ApplicationException("The photo cannot be updated. The profile update is cancelled.");
        }
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