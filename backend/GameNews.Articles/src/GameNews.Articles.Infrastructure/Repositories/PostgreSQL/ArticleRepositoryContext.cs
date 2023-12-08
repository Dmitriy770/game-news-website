using GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL;

public sealed class ArticleRepositoryContext : DbContext
{
    public DbSet<ArticleEntity> Articles { get; set; } = null!;
    public DbSet<TagEntity> Tags { get; set; } = null!;

    public ArticleRepositoryContext()
    {
        Database.EnsureCreated();
    }
}