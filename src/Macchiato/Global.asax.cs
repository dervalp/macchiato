using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Macchiato
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : Sitecore.Web.Application
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        {
            string frameworkVersion = this.GetFrameworkVersion();
            if (!string.IsNullOrEmpty(frameworkVersion) && frameworkVersion.StartsWith("v4.", StringComparison.InvariantCultureIgnoreCase))
            {
                args.User = Sitecore.Context.User;
            }
        }

        string GetFrameworkVersion()
        {
            try
            {
                return System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Cannot get framework version", ex, this);
                return string.Empty;
            }
        }
    }
}