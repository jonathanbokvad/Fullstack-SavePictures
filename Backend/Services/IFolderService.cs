using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApiToDatabase.Services
{
    public interface IFolderService
    {
        Task<List<Folder>> GetFolders();
        Task<OkResult> CreateFolder(FolderRequest folderRequest);
        Task<UpdateResult> UpdateFolderName(FolderRequest folderRequest);
        Task<DeleteResult> DeleteFolder(string folderId);
    }
}
