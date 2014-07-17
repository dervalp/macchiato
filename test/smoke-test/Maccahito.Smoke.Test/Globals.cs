using System;

namespace Macchiato.Smoke.Test
{
  internal static class Globals
  {
    internal static class TestSite
    {
      internal static string BaseUrl
      {
        get
        {
          var websiteUrl = Environment.GetEnvironmentVariable("TestWebsiteUrl");

          return string.IsNullOrEmpty(websiteUrl) ? "https://aaa" : websiteUrl;
        }
      }

      internal static string UrlStem = "sitecore/api/ssc";
    }
  }
}