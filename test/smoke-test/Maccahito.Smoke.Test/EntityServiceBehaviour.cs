using Should;
using Xunit;

namespace Macchiato.Smoke.Test
{
  public class EntityServiceBehaviour : BrowserBehaviourTest
  {
    private readonly string _baseUrl;

    public EntityServiceBehaviour()
    {
      Browser.Accept = "application/json";

      _baseUrl = UrlHelper.BuildApiUrl("speak-sample/speaktest");
    }

    [Fact]
    public void Should_return_not_found_error_when_specific_entity_is_missing()
    {
      Browser.Navigate(_baseUrl + "/fetchentity/51f872c0-882a-4491-b275-bf96af401a5e");

      Browser.LastWebException.ShouldNotBeNull();
      Browser.LastWebException.Message.ShouldContain("(404) Not Found");
    }
  }
}