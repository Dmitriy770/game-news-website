using Xunit;

namespace GameNews.Articles.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public class FixtureDefinition : IClassFixture<TestFixture>
{
}