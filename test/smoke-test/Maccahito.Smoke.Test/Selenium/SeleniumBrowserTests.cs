using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Macchiato.Smoke.Test.Selenium
{
    public abstract class SeleniumBrowserTests : IDisposable
    {
        protected readonly IWebDriver Driver;

        protected SeleniumBrowserTests()
        {
            Driver = new FirefoxDriver();
        }

        protected string SiteBaseUrl { get { return GetSiteBaseUrl(); } }

        private static string GetSiteBaseUrl()
        {
            var baseUrl = Globals.TestSite.BaseUrl;
            
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }
            return baseUrl;
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
