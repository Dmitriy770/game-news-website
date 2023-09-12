using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Interfaces;

public interface IArticleRepository
{
    public Task<ArticleModel> Get(long id, CancellationToken token);
    public Task<IEnumerable<DescriptionModel>> GetDescriptions(int take, int skip, CancellationToken token);
    public Task<long> Add(ArticleModel article, CancellationToken token);
    public Task Delete(long id, CancellationToken token);
}