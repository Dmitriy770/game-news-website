using FluentResults;
using GameNews.Articles.Domain.DTOs;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Services.Interfaces;

public interface ITagService
{
    public Task<Result<Guid>> Save(TagModel tag, UserModel user, CancellationToken cancellationToken);

    public Task<Result> Update(UpdateTagDto tagDto, UserModel user, CancellationToken cancellationToken);

    public Task<Result> Delete(Guid id, UserModel user, CancellationToken cancellationToken);

    public Task<Result<TagModel>> GetById(Guid id, CancellationToken cancellationToken);

    public IAsyncEnumerable<TagModel> GetAll(CancellationToken cancellationToken);

}