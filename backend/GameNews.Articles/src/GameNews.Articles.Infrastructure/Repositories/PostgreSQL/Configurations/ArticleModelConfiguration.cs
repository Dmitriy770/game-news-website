using GameNews.Articles.Domain.Models;
using GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Configurations;

public class ArticleModelConfiguration : IEntityTypeConfiguration<ArticleModel>
{
    public void Configure(EntityTypeBuilder<ArticleModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasMany(e => e.Tags)
            .WithMany(e => e.Articles);
    }
}