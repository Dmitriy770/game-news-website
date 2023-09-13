using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Services.Interfaces;

namespace GameNews.Articles.Domain.Services;

public class ArticleService : IArticleService
{
    private IArticleRepository _articleRepository;
    
    public ArticleService(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }
    
    public async Task<ArticleModel> Get(long id, CancellationToken token)
    {
        return await _articleRepository.Get(id, token);
    }

    public async Task<IEnumerable<DescriptionModel>> GetDescriptions(int take, int skip, CancellationToken token)
    {
        return await _articleRepository.GetDescriptions(take, skip, token);
    }

    public async Task<long> Add(ArticleModel article, CancellationToken token)
    {
        return await _articleRepository.Add(article, token);
    }

    public async Task Delete(long id, CancellationToken token)
    {
        await _articleRepository.Delete(id, token);
    }
}