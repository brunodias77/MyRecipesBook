using Azure.Storage.Blobs;
using MRB.Domain.Entities;
using MRB.Domain.Services.Storage;

namespace MRB.Infra.Services.Storage;

public class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public Task Upload(User user, Stream file, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFileUrl(User user, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task Delete(User user, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteContainer(Guid userIdentifier)
    {
        throw new NotImplementedException();
    }
}