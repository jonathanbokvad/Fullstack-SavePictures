namespace ApiToDatabase.Models.RequestModels;

public class CreateFolderRequest
{
    public string FolderId { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}