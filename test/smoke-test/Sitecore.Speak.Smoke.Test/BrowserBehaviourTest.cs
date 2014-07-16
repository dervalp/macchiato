using System;
using SimpleBrowser;

namespace Sitecore.Speak.Smoke.Test
{
    public abstract class BrowserBehaviourTest : IDisposable
    {
        protected readonly Browser Browser;

        protected BrowserBehaviourTest()
        {
            Browser = new Browser();
        }

        public void Dispose()
        {
            Browser.Close();
        }
    }
}