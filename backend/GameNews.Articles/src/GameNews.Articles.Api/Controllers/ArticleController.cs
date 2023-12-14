using GameNews.Articles.Api.Requests.V1;
using GameNews.Articles.Api.Responses.V1;
using Microsoft.AspNetCore.Mvc;

namespace GameNews.Articles.Api.Controllers;

[ApiController]
[Route("article")]
public class ArticleController : ControllerBase
{
    
    [HttpGet("{id}")]
    public async Task<GetArticleResponse> Get(long id, CancellationToken ct)
    {
        // var article = await _articleService.Get(id, ct);

        return new GetArticleResponse
        {
        };
    }

    [HttpPost]
    public async Task<AddArticleResponse> Add(AddArticleRequest request, CancellationToken ct)
    {
        return new AddArticleResponse
        {
        };
    }

    [HttpDelete("{id}")]
    public async Task<DeleteArticleResponse> Delete(long id, CancellationToken ct)
    {

        return new DeleteArticleResponse();
    }

    [HttpGet("descriptions")]
    public async Task<GetDescriptionsResponse> GetDescriptions(
        [FromQuery] long take,
        [FromQuery] long skip,
        CancellationToken ct)
    {
        return new GetDescriptionsResponse
        {
        };
    }
}