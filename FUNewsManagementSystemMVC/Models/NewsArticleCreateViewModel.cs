using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class NewsArticleCreateViewModel
{
    [Required]
    public string NewsTitle { get; set; }

    [Required]
    public string Headline { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }
    public bool? NewsStatus { get; set; }

    [Display(Name = "Category")]
    public short? CategoryId { get; set; }

    [Display(Name = "Tags")]
    public List<string> SelectedTags { get; set; } = new List<string>();

    public IEnumerable<SelectListItem> AvailableTags { get; set; }
}
