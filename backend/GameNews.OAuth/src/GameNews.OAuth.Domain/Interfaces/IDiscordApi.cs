using GameNews.OAuth.Domain.Models;
using GameNews.OAuth.Infrastructure.Api.Entities;

namespace GameNews.OAuth.Domain.Interfaces;

public interface IDiscordApi
{
    public Task<AccessTokenModel> GetAccessToken(string code, CancellationToken cancellationToken);
    
    public Task RevokeAccessToken(string accessToken, CancellationToken cancellationToken);

    public Task<AccessTokenModel> RefreshAccessToken(string accessToken, CancellationToken cancellationToken);
    
    public Task<UserEntity> GetUser(string accessToken, CancellationToken cancellationToken);

    public Task<List<GuildEntity>> GetUserGuilds(string accessToken, CancellationToken cancellationToken);

    public Task<GuildMemberEntity> GetGuildMember(string accessToken, CancellationToken cancellationToken);


}