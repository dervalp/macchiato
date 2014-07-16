using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

using Sitecore.Services.Core.Model;

namespace Sitecore.Speak.Smoke.Test
{
  public abstract class EntityServiceTest<T> : IDisposable where T : EntityIdentity
  {
    protected readonly T EntityInstance;
    protected readonly string BaseUrl;
    protected readonly Request Request;

    protected readonly IList<T> ItemsToRemove;

    protected EntityServiceTest(string relativeUrl, T entityInstance)
    {
      EntityInstance = entityInstance;
      BaseUrl = Globals.TestSite.BaseUrl + relativeUrl;
      Request = new Request();
      ItemsToRemove = new List<T>();
    }

    protected T[] FetchEntities()
    {
      var response = Request.Execute(BaseUrl, null, "GET").ToString();
      return JsonConvert.DeserializeObject<T[]>(response);
    }

    protected T GetEntity(string entityId)
    {
      var response = Request.Execute(BaseUrl + "/" + entityId, null, "GET").ToString();
      return JsonConvert.DeserializeObject<T>(response);
    }

    protected HttpResponse Options()
    {
      return (HttpResponse)Request.Execute<HttpResponse>(BaseUrl, null, "OPTIONS");
    }

    public void Dispose()
    {
      foreach (var item in ItemsToRemove)
      {
        DeleteEntity(item);
      }
    }

    protected void DeleteEntity(T entity)
    {
      Request.Execute<HttpResponse>(BaseUrl, entity, "DELETE");
    }

    protected static string GetIdentityFromLocationHeader(string header)
    {
      return header.Split(new[] { '/' }).Last();
    }
  }
}