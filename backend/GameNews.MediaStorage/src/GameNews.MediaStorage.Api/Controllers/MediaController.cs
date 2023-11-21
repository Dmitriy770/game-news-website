using Microsoft.AspNetCore.Mvc;

namespace GameNews.MediaStorage.Api.Controllers;

[ApiController]
[Route("media-files")]
public sealed class MediaController : ControllerBase
{
    [HttpPost("articleId:guid")]
    public Task<IActionResult> Save(
        [FromRoute] Guid articleId,
        [FromRoute] Guid fileId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("articleId:guid/fileId:guid")]
    public Task<IActionResult> Get(
        [FromRoute] Guid articleId,
        [FromRoute] Guid fileId,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    [HttpGet("articleId:guid")]
    public Task<IActionResult> GetFilesInfoByArticleId(
        [FromRoute] Guid articleId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("articleId:guid/fileId:guid")]
    public Task<IActionResult> Update(
        [FromRoute] Guid articleId,
        [FromRoute] Guid fileId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("articleId:guid/fileId:guid")]
    public Task<IActionResult> Delete(
        [FromRoute] Guid articleId,
        [FromRoute] Guid fileId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("\"articleId:guid")]
    public Task<IActionResult> DeleteByArticleId(
        [FromRoute] Guid articleId,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}