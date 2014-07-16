using System;
using System.Net;
using System.Web;
using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.Authentication
{
  public class ServicesAuthenticationBehaviour
  {
    private readonly string _baseUrl;
    private readonly Request _request;

    private const string ValidLoginCredentials = @"{
                                                      ""domain"": ""sitecore"",
                                                      ""username"": ""admin"",
                                                      ""password"": ""b""
                                                  }";

    public ServicesAuthenticationBehaviour()
    {
      _baseUrl = UrlHelper.BuildApiUrl("auth");
      _request = new Request();
    }

    [Fact]
    public void Should_receive_a_internal_server_error_response_to_auth_login_over_http()
    {
      _request.Execute<HttpResponse>(_baseUrl + "/login", ValidLoginCredentials, "POST", "application/json");

      _request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
      _request.HttpResponse.Cookies[".ASPXAUTH"].ShouldBeNull();
    }

    [Fact]
    public void Should_be_able_to_login()
    {
      var loginUrl = new UriBuilder(_baseUrl + "/login")
      {
        Scheme = Uri.UriSchemeHttps,
        Port = -1
      };

      _request.Execute<HttpResponse>(loginUrl.ToString(), ValidLoginCredentials, "POST", "application/json");

      _request.HttpResponse.Cookies[".ASPXAUTH"].ShouldNotBeNull();
    }

    [Fact]
    public void Should_be_able_to_logout()
    {
      _request.Execute<HttpResponse>(_baseUrl + "/logout", null, "POST");

      _request.HttpResponse.Cookies[".ASPXAUTH"].ShouldBeNull();
    }
  }
}