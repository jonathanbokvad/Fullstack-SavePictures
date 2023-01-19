using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using MongoDB.Driver;

namespace ApiToDatabase.Services
{
    public interface IPictureService
    {
        Task<List<Picture>> GetPictures(string folderId);
        Task<DeleteResult> DeletePicture(string pictureId);
        Task<Folder> CreatePicture(PictureRequest pictureRequest);
        Picture CreatePictureTest(byte[] binaryData, string name);
    }
}
