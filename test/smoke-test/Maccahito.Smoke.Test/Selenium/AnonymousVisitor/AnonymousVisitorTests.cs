using Should;
using Xunit;

namespace Macchiato.Smoke.Test.Selenium.AnonymousVisitor
{
    public class AnonymousVisitorTests : SeleniumBrowserTests
    {
        [Fact]
        public void SiteLoads()
        {
            Driver.Navigate().GoToUrl(SiteBaseUrl);
            Driver.Title.ShouldEqual("Welcome to Sitecore");
        }
    }
}