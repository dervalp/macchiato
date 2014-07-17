using System.Linq;
using Should;
using Xunit;

namespace Macchiato.Smoke.Test.ItemService.Search
{
    public class ItemServiceSearchLanguageBehaviour : ItemServiceSearchTest
    {
        [Fact]
        public void Search_returns_home_page_in_context_language_of_user()
        {
            RunSearch().Results.Single()["ItemLanguage"].ShouldEqual("en");
        }

        [Fact]
        public void Search_returns_home_page_in_japanese()
        {
            Url = Url + "&language=ja-JP";

            RunSearch().Results.Single()["ItemLanguage"].ShouldEqual("ja-JP");
        }

        [Fact]
        public void Search_returns_home_page_for_all_languages()
        {
            Url = Url + "&language=all";

            RunSearch().Results.Count().ShouldBeGreaterThan(1);
        }
    }
}