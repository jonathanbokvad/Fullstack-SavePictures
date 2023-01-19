using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
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
