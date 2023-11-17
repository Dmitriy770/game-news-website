using GameNews.OAuth.Domain.Entities;
using GameNews.OAuth.Domain.Models;

namespace GameNews.OAuth.Domain.Interfaces;

public interface IDiscordClient
{
    public Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken);
    
    public Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken);

    public Task<AccessTokenModel> RefreshAccessToken(string accessToken, CancellationToken cancellationToken);
    
    public Task<DiscordUserModel> GetUser(string accessToken, CancellationToken cancellationToken);

    public Task<IEnumerable<DiscordGuildModel>> GetUserGuilds(string accessToken, CancellationToken cancellationToken);

    public Task<DiscordGuildMemberModel?> GetGuildMember(string accessToken, CancellationToken cancellationToken);


}