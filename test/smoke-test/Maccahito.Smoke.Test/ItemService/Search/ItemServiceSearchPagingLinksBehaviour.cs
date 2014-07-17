using Should;
using Xunit;

namespace Macchiato.Smoke.Test.ItemService.Search
{
  public class ItemServiceSearchPagingLinksBehaviour : ItemServiceSearchPagedResultsTest
  {
    [Fact]
    public void Should_have_a_next_link_from_first_page_of_multipage_results_set()
    {
      var itemResults = RunSearch(Url);
      itemResults.Links[0].Rel.ShouldEqual("nextPage");
      itemResults.Links[0].Href.Contains("&page=1").ShouldBeTrue();
    }

    [Fact]
    public void Should_have_a_prev_link_from_second_page_of_multipage_results_set()
    {
      var itemResults = RunSearch(Url + "&page=1");
      itemResults.Links[0].Rel.ShouldEqual("prevPage");
    }

    [Fact]
    public void Should_have_a_next_link_from_second_page_of_multipage_results_set()
    {
      var itemResults = RunSearch(Url + "&page=1");
      itemResults.Links[1].Rel.ShouldEqual("nextPage");
    }

    [Fact]
    public void Should_treat_page_0_same_as_page_1()
    {
      var itemResults = RunSearch(Url + "&page=0");
      itemResults.Links[0].Rel.ShouldEqual("nextPage");
      itemResults.Links[0].Href.Contains("&page=1").ShouldBeTrue();
    }
  }
}