namespace Macchiato.Smoke.Test.ItemService.Search
{
  public class ItemServiceSearchPagedResultsTest : ItemServiceSearchTest
  {
    public ItemServiceSearchPagedResultsTest()
    {
      Url = string.Format("{0}/search?term={1}&database=core", BaseUrl, "sitecore");
    }  
  }
}