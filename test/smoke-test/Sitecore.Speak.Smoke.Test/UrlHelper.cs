namespace Sitecore.Speak.Smoke.Test
{
  internal static class UrlHelper
  {
    internal static string BuildApiUrl(string path = "")
    {
      return string.Format("{0}/{1}/{2}", Globals.TestSite.BaseUrl, Globals.TestSite.UrlStem, path);
    }

    internal static string BuildApiRelativeUrl(string path = "")
    {
      return string.Format("/{0}/{1}", Globals.TestSite.UrlStem, path);
    }
  }
}