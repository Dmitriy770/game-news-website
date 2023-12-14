using FluentResults;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Services.Interfaces;

public interface ITagService
{
    public Task<Result> Save(TagModel tag, UserModel user, CancellationToken cancellationToken);
}