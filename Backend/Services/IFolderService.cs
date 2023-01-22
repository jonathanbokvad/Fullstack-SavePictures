using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiToDatabase.Services
{
    public interface IFolderService
    {
        Task<List<Folder>> GetFolders(string userId);
        Task<OkResult> CreateFolder(CreateFolderRequest createFolderRequest);
        Task<UpdateResult> UpdateFolderName(UpdateFolderRequest folderRequest);
        Task<DeleteResult> DeleteFolder(string folderId);
    }
}
