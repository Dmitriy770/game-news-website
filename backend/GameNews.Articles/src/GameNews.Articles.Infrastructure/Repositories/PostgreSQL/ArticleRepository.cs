using FluentResults;
using GameNews.Articles.Domain.Errors;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Infrastructure.Exceptions;
using GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL;

public sealed class ArticleRepository(
    ArticleRepositoryContext dbContext
) : IArticleRepository
{
    public async Task<Result> SaveTag(TagModel tag, CancellationToken cancellationToken)
    {
        if (await dbContext.Tags.FindAsync(tag.Id, cancellationToken) is null)
        {
            await dbContext.Tags.AddAsync(new TagEntity { Id = tag.Id, Name = tag.Name, Description = tag.Description },
                cancellationToken);
        }
        else
        {
            await dbContext.Tags.Where(t => t.Id == tag.Id)
                .ExecuteUpdateAsync(s => s
                        .SetProperty(t => t.Name, tag.Name)
                        .SetProperty(t => t.Description, tag.Description),
                    cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await dbContext.Tags.Where(t => t.Id == tagId).ExecuteDeleteAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<TagModel>> GetTagById(Guid id, CancellationToken cancellationToken)
    {
        var tagEntity = await dbContext.Tags.FindAsync(id, cancellationToken);

        if (tagEntity is null)
        {
            return Result.Fail(new TagNotFoundError());
        }

        var result = TagModel.Create(tagEntity.Id, tagEntity.Name, tagEntity.Description);
        if (result.IsFailed)
        {
            throw new InvalidDataInDbException();
        }

        return result.Value;
    }

    public async IAsyncEnumerable<TagModel> GetAllTags(CancellationToken cancellationToken)
    {
        await foreach (var tag in dbContext.Tags)
        {
            var result = TagModel.Create(tag.Id, tag.Name, tag.Description);
            if (result.IsFailed)
            {
                throw new InvalidDataInDbException();
            }

            yield return result.Value;
        }
    }
}