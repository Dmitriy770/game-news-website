using GameNews.Articles.Domain.Models;
using GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL;

public sealed class ArticleRepositoryContext : DbContext
{
    public DbSet<ArticleModel> Articles { get; set; } = null!;
    public DbSet<TagModel> Tags { get; set; } = null!;

    public ArticleRepositoryContext(
        DbContextOptions<ArticleRepositoryContext> options
    ) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArticleModelConfiguration).Assembly);
    }
}