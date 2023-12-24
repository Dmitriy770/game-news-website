using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;

[Table("tag")]
public class TagEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ArticleModel> Articles { get; set; } = [];
}