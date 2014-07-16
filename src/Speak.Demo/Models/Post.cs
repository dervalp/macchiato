using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Services.Core.Model;

namespace Macchiato.Model
{
  public class Post : EntityIdentity
  {
    [Required]
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Guid> Categories { get; set; }
    public Guid Author { get; set; }
    [EmailAddress]
    public string AuthorEmail { get; set; }
    public string Status { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastUpdate { get; set; }
  }
}