using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.ItemService.Search
{
  public class ItemServiceSearchResultsCountsBehaviour : ItemServiceSearchPagedResultsTest
  {
    [Fact]
    public void Should_have_a_total_count_greater_than_zero()
    {
      var itemResults = RunSearch(Url);
      itemResults.TotalCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Should_have_a_total_page_greater_than_zero()
    {
      var itemResults = RunSearch(Url);
      itemResults.TotalPage.ShouldBeGreaterThan(0);
    }
  }
}