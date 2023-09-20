using GameNews.Articles.Api.Requests.V1;
using GameNews.Articles.Api.Responses.V1;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers.V1;

[ApiController]
[Route("api/v1/article")]
public class ArticleController : ControllerBase
{
    private IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet("{id}")]
    public async Task<GetArticleResponse> Get(long id, CancellationToken ct)
    {
        var article = await _articleService.Get(id, ct);

        return new GetArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Image = article.Image,
            PublicationDate = article.PublicationDate,
            Content = article.Content
        };
    }

    [HttpPost]
    public async Task<AddArticleResponse> Add(AddArticleRequest request, CancellationToken ct)
    {
        var id = await _articleService.Add(new ArticleModel
        {
            Content = request.Content,
            Title = request.Title,
            PublicationDate = request.PublicationDate,
            Image = request.Image
        }, ct);

        return new AddArticleResponse
        {
            Id = id
        };
    }

    [HttpDelete("{id}")]
    public async Task<DeleteArticleResponse> Delete(long id, CancellationToken ct)
    {
        await _articleService.Delete(id, ct);

        return new DeleteArticleResponse();
    }

    //TODO Заменить int на long у take, skip
    [HttpGet("descriptions")]
    public async Task<GetDescriptionsResponse> GetDescriptions([FromQuery] int take, [FromQuery] int skip,
        CancellationToken ct)
    {
        var descriptions = await _articleService.GetDescriptions(take, skip, ct);

        return new GetDescriptionsResponse
        {
            Descriptions = descriptions.Select(d => new Description
            {
                Id = d.Id,
                Title = d.Title,
                Image = d.Image,
                PublicationDate = d.PublicationDate
            })
        };
    }
}