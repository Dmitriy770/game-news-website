namespace GameNews.MediaStorage.Domain.Models;

public record UserModel
{
    public string Id { get;}
    public string Role { get;}
    
    public UserModel(string Id, string Role)
    {
        this.Id = Id;

        this.Role = Role is not ("User" or "Author" or "Administrator") ? "User" : Role;
    }
}