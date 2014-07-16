using System;
using System.Collections.Generic;
using System.Linq;

using Sitecore.Services.Core;
using Speak.Blog.Model;

namespace Speak.Blog.Data
{
  public class PostRepository : IRepository<Post>
  {
    private static readonly IList<Post> Entities;

    static PostRepository()
    {
      Entities = new List<Post>();
    }

    public IQueryable<Post> GetAll()
    {
      return Entities.AsQueryable();
    }

    public Post FindById(string id)
    {
      return Entities.FirstOrDefault(x => x.Id == id);
    }

    public void Add(Post entity)
    {
      entity.Id = Guid.NewGuid().ToString();
      Entities.Add(entity);
    }

    public bool Exists(Post entity)
    {
      return Entities.Any(x => x.Id.Equals(entity.Id, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Update(Post entity)
    {
      var entityToUpdate = Entities.SingleOrDefault(x => x.Id == entity.Id);

      if (entityToUpdate == null) throw new InvalidOperationException("Cannot update entity");

      entityToUpdate = entity;
    }

    public void Delete(Post entity)
    {
      var entitiesToDelete = Entities.Where(x => x.Id.Equals(entity.Id, StringComparison.InvariantCultureIgnoreCase)).ToArray();

      foreach (var entityToDelete in entitiesToDelete)
      {
        Entities.Remove(entityToDelete);
      }
    }
  }
}