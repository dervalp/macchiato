using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Services.Core.Model;

namespace Sitecore.Services.Samples.Model
{
  public class SimpleData : EntityIdentity
  {
    [Required(ErrorMessage = "Value is required")]
    [StringLength(50, ErrorMessage = "Must be less than 50 characters in length")]
    public string Value { get; set; }

    [Required(ErrorMessage = "Value is required")]
    [StringLength(5, ErrorMessage = "Must be less than 5 characters in length")]
    [RegularExpression("[A-Za-z_][A-Za-z_0-9]*.", ErrorMessage = "Must be alphanumeric string")]
    public string Payload { get; set; }

    public List<Guid> RelatedIds { get; set; }
    public IEnumerable<string> RelatedStrings { get; set; }
    public double[] RelatedNumbers { get; set; }
  }
}