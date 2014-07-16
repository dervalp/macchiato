using Sitecore.Services.Core.Model;

namespace Sitecore.Speak.Smoke.Test.ItemService.Search
{
    public abstract class ItemServiceSearchTest : ItemServiceTest
    {
        protected string Url;

        protected ItemServiceSearchTest()
        {
            Url = string.Format("{0}/search?term={1}&database=master", BaseUrl, "Home");            
        }

        protected SearchItemResults RunSearch()
        {
          return RunSearch(Url);
        }

        protected SearchItemResults RunSearch(string url)
        {
          return (SearchItemResults)Request.Execute<SearchItemResults>(url, null, "GET");
        }
    }
}