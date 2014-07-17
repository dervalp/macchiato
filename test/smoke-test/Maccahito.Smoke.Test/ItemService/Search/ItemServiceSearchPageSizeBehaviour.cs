using Should;
using Xunit;

namespace Macchiato.Smoke.Test.ItemService.Search
{
  public class ItemServiceSearchPageSizeBehaviour : ItemServiceSearchPagedResultsTest
  {
    [Fact]
    public void Should_return_total_page_equals_total_count_when_page_size_is_1()
    {
      var itemResults = RunSearch(Url + "&pageSize=1");
      itemResults.TotalCount.ShouldEqual(itemResults.TotalPage);
    }

    [Fact]
    public void Should_default_page_size_when_zero_is_requested()
    {
      var defaultItemResults = RunSearch(Url);

      var itemResults = RunSearch(Url + "&pageSize=0");

      itemResults.TotalCount.ShouldEqual(defaultItemResults.TotalCount);
      itemResults.TotalPage.ShouldEqual(defaultItemResults.TotalPage);
    }
  }
}