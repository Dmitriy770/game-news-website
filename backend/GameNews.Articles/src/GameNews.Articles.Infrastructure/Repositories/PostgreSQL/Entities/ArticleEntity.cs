using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;

[Table("article")]
public class ArticleEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.Empty;
    public string? Title { get; set; }
    public DateTime? CreationDate { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = false;
    public string? PreviewText { get; set; }
    public Guid? PreviewImageIdd { get; set; }
    public List<TagEntity> Tags { get; set; } = new();
}