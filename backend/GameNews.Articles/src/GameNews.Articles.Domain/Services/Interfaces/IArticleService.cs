using FluentResults;
using GameNews.Articles.Domain.DTOs;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Services.Interfaces;

public interface IArticleService
{
    public Task<Result<ArticleModel>> Create(
        ArticleModel article,
        UserModel user,
        CancellationToken cancellationToken);

    public Task<Result<ArticleModel>> Update(
        UpdateArticleDto updateArticleDto,
        UserModel user,
        CancellationToken cancellationToken
    );

    public Task<Result> Delete(Guid id, UserModel user, CancellationToken cancellationToken);

    public Task<Result<ArticleModel>> GetArticle(Guid id, CancellationToken cancellationToken);
}