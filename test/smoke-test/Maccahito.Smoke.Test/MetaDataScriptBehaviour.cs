using Should;
using Xunit;

namespace Macchiato.Smoke.Test
{
  public class MetaDataScriptBehaviour : BrowserBehaviourTest
  {
    private readonly string _baseUrl;

    public MetaDataScriptBehaviour()
    {
      _baseUrl = UrlHelper.BuildApiUrl("script/metadata");
    }

    [Fact]
    public void Should_return_bad_request_if_no_scripts_requested()
    {
      Browser.Navigate(_baseUrl);

      Browser.LastWebException.ShouldNotBeNull();
      Browser.LastWebException.Message.ShouldContain("(400) Bad Request");
    }

    [Fact]
    public void Should_return_scripts_specified_in_query_string()
    {
      Browser.Navigate(_baseUrl + "?scripts=emailAddress");

      Browser.ResponseText.Contains("EntityService.utils.validator.add").ShouldBeTrue();
    }

    [Fact]
    public void Should_return_not_found_if_script_does_not_exist()
    {
      Browser.Navigate(_baseUrl + "?scripts=emailAddress,foo");

      Browser.LastWebException.ShouldNotBeNull();
      Browser.LastWebException.Message.ShouldContain("(404) Not Found");
    }
  }
}