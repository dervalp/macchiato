using System;
using System.Collections.Generic;
using System.Linq;

using Sitecore.Services.Core;
using Sitecore.Services.Samples.Model;

namespace Sitecore.Services.Samples.Data
{
  public class FakeEntityRepository : IRepository<SimpleData>
  {
    private static readonly IList<SimpleData> Entities;

    static FakeEntityRepository()
    {
      Entities = new List<SimpleData>
                {
                    new SimpleData
                        {
                            Id = Guid.NewGuid().ToString(), 
                            Value = "Item 1", 
                            Payload = "ab",
                            RelatedIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()},
                            RelatedStrings = new List<string> { "string 1", "string 2"},
                            RelatedNumbers = new[] { 0.1, 0.123, 1.34 }
                        },
                    new SimpleData
                        {
                            Id = new Guid("51f872c0-882a-4491-b275-bf96af401a5f").ToString(), 
                            Value = "Item 2",
                            Payload = "bcd",
                            RelatedIds = new List<Guid>(),
                            RelatedStrings = new List<string>(),
                            RelatedNumbers = new double[] { }
                        },
                    new SimpleData { 
                            Id = Guid.NewGuid().ToString(), Value = "Item 3",
                            Payload = "cdef"},
                };
    }

    public IQueryable<SimpleData> GetAll()
    {
      return Entities.AsQueryable();
    }

    public SimpleData FindById(string id)
    {
      return Entities.FirstOrDefault(x => x.Id == id);
    }

    public void Add(SimpleData entity)
    {
      entity.Id = Guid.NewGuid().ToString();
      Entities.Add(entity);
    }

    public bool Exists(SimpleData entity)
    {
      return Entities.Any(x => x.Value.Equals(entity.Value, StringComparison.InvariantCultureIgnoreCase));
    }

    public void Update(SimpleData entity)
    {
      var entityToUpdate = Entities.SingleOrDefault(x => x.Id == entity.Id);

      if (entityToUpdate == null) throw new InvalidOperationException("Cannot update entity");

      entityToUpdate.Payload = entity.Payload;
      entityToUpdate.Value = entity.Value;
    }

    public void Delete(SimpleData entity)
    {
      var entitiesToDelete = Entities.Where(x => x.Value.Equals(entity.Value, StringComparison.InvariantCultureIgnoreCase)).ToArray();

      foreach (var entityToDelete in entitiesToDelete)
      {
        Entities.Remove(entityToDelete);
      }
    }
  }
}