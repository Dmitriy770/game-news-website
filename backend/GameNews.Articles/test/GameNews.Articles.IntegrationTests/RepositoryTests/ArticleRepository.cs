using Bogus;
using FluentAssertions;
using GameNews.Articles.Domain.Interfaces;
using GameNews.Articles.Domain.Models;
using GameNews.Articles.IntegrationTests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace GameNews.Articles.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class ArticleRepository
{
    private readonly IArticleRepository _articleRepository;

    public ArticleRepository(TestFixture fixture)
    {
        _articleRepository = fixture.ArticleRepository;
    }

    [Fact]
    public async Task Add_AddArticle_Success()
    {
        // Arrange
        var faker = new Faker();
        
        var article = new ArticleModel
        {
            Id = faker.Random.Long(),
            Title = faker.Random.String2(1000),
            PublicationDate = faker.Date.Recent(),
            Image = faker.Random.String2(1000),
            Content = faker.Random.String2(1000)
        };

        // Act, Assert
        await _articleRepository.Add(article, CancellationToken.None);
    }
    
    [Fact]
    public async Task Get_GetArticle_Success()
    {
        // Arrange
        var faker = new Faker();
        
        var article = new ArticleModel
        {
            Title = faker.Random.String2(1000),
            PublicationDate = faker.Date.Recent(),
            Image = faker.Random.String2(1000),
            Content = faker.Random.String2(1000)
        };

        var id = await _articleRepository.Add(article, CancellationToken.None);
        
        // Act
        var dbArticle = await _articleRepository.Get(id, CancellationToken.None);
        
        // Assert
        dbArticle.Id.Should().Be(id);
        dbArticle.Title.Should().Be(article.Title);
        dbArticle.PublicationDate.Should().BeSameDateAs(article.PublicationDate);
        dbArticle.Image.Should().Be(article. Image);
        dbArticle.Content.Should().Be(article.Content);

    }

    [Fact]
    public async Task Delete_DeleteArticle_Success()
    {
        // Arrange
        var faker = new Faker();
        
        var article = new ArticleModel
        {
            Title = faker.Random.String2(1000),
            PublicationDate = faker.Date.Recent(),
            Image = faker.Random.String2(1000),
            Content = faker.Random.String2(1000)
        };

        var id = await _articleRepository.Add(article, CancellationToken.None);
        
        // Act, Assert
        await _articleRepository.Delete(id, CancellationToken.None);
    }
    
    [Fact]
    public async Task GetDescription_GetDescription_Success()
    {
        // Arrange
        var faker = new Faker();
        
        var article = new ArticleModel
        {
            Title = faker.Random.String2(1000),
            PublicationDate = faker.Date.Recent(),
            Image = faker.Random.String2(1000),
            Content = faker.Random.String2(1000)
        };

        var id = await _articleRepository.Add(article, CancellationToken.None);
        
        // Act
        var descriptions = await _articleRepository.GetDescriptions(1, 0, CancellationToken.None);
        var description = descriptions.First();

        //Assert
        description.Id.Should().Be(id);
        description.Title.Should().Be(article.Title);
        description.PublicationDate.Should().BeSameDateAs(article.PublicationDate);
        description.Image.Should().Be(article.Image);
    }
}