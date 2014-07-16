using System.Web.Http;

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(SitecoreCms.WebApiConfig), "PostStart")]

namespace SitecoreCms
{
    public static class WebApiConfig
    {
        // Dev. Note: The SPEAK customisations to the Web Api configuration are performed in an initialization pipeline processor currently

        public static void PostStart()
        {
            Register(GlobalConfiguration.Configuration);
        }

        private static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApiRoute",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //
            // requires PM> install-package Microsoft.AspNet.WebApi.Tracing 
            config.EnableSystemDiagnosticsTracing();
        }
    }
}