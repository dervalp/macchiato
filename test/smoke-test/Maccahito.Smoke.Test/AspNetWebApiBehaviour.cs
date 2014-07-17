using System;
using Should;
using Xunit;

namespace Macchiato.Smoke.Test
{
    public class AspNetWebApiBehaviour : BrowserBehaviourTest
    {
        [Fact]
        public void Should_return_array_of_products()
        {
            Browser.Navigate(Globals.TestSite.BaseUrl + "/api/products");

            Browser.ResponseText.ShouldContain("[{\"Id\":1,");
        }

        [Fact]
        public void Should_return_product_3()
        {
            Console.WriteLine("BaseUrl = " + Globals.TestSite.BaseUrl);

            Browser.Navigate(Globals.TestSite.BaseUrl + "/api/products/3");

            Browser.ResponseText.ShouldContain("{\"Id\":3,");
        }
    }
}