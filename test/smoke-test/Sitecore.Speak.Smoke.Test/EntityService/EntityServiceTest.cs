namespace Sitecore.Speak.Smoke.Test.EntityService
{
  public abstract class EntityServiceTest
  {
    protected string BaseUrl;
    protected Request Request;

    protected EntityServiceTest()
    {
      BaseUrl = UrlHelper.BuildApiUrl();
      Request = new Request();
    }
  }
}