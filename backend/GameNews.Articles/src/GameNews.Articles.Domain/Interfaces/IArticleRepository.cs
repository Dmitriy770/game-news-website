using FluentResults;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Interfaces;

public interface IArticleRepository
{
    public Task<Result> SaveTag(TagModel tag, CancellationToken cancellationToken);
}