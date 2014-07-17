using System.Linq;
using Should;
using Xunit;

namespace Macchiato.Smoke.Test.ItemService.Search
{
    public class ItemServiceSearchFacetBehaviour : ItemServiceSearchTest
    {
        [Fact]
        public void Search_returns_facets()
        {
            RunSearch().Facets.ShouldNotBeNull();
        }

        [Fact]
        public void Search_returns_templatename_facet()
        {
            RunSearch().Facets.First().Name.ShouldEqual("_templatename");
        }

        [Fact]
        public void Search_filters_results_by_single_facet_in_request()
        {
            Url = string.Format("{0}/search?term=sitecore&database=master&facet=_templatename|condition", BaseUrl); 
            
            RunSearch().Facets.Single(x => x.Name == "_templatename")
                              .Values.Count().ShouldEqual(1);
        }

        [Fact]
        public void Search_facets_contain_a_populated_link_property()
        {
            Url = string.Format("{0}/search?term=sitecore&database=master&facet=_templatename|condition", BaseUrl);

            var link = RunSearch().Facets.Single(x => x.Name == "_templatename")
                                         .Values.First().Link;

            link.Href.Contains(System.Uri.EscapeUriString("&facet=_templatename|condition")).ShouldBeTrue();
            link.Method.ShouldEqual("GET");
            link.Rel.ShouldEqual("_templatename|condition");
        }
    }
}