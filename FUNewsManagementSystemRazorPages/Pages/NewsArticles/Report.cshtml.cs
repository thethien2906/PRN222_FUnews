using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;
using Services.DTOs;
using Services.IService;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsManagementSystemRazorPages.Pages.NewsArticles
{
    public class ReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;

        public ReportModel(INewsArticleService newsArticleService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
        }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new List<NewsArticleDTO>();
        public SelectList Categories { get; set; }
        public string ChartDataJson { get; set; }
        public string PieChartDataJson { get; set; }

        public void OnGet(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = DateTime.Now.AddMonths(-1);
                endDate = DateTime.Now;
            }

            ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");

            // Lấy danh sách bài báo
            var articles = _newsArticleService
                .GetNewsArticlesByPeriod(startDate.Value, endDate.Value)
                .OrderBy(a => a.CreatedDate)
                .ToList();

            // Lấy danh mục và ánh xạ CategoryId -> CategoryName
            var categories = _categoryService.GetCategories()
                .ToDictionary(c => c.CategoryId, c => c.CategoryName);

            foreach (var article in articles)
            {
                if (article.CategoryId.HasValue && categories.TryGetValue(article.CategoryId.Value, out string categoryName))
                {
                    article.CategoryName = categoryName;
                }
            }

            NewsArticles = articles;

            // Chuẩn bị dữ liệu biểu đồ cột (số lượng bài viết theo ngày)
            var chartData = NewsArticles
                .Where(a => a.CreatedDate.HasValue)
                .GroupBy(a => a.CreatedDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                }).ToList();

            ChartDataJson = JsonConvert.SerializeObject(chartData);

            // Dữ liệu cho biểu đồ tròn (Phân loại bài viết theo danh mục)
            var pieChartData = NewsArticles
                .Where(a => !string.IsNullOrEmpty(a.CategoryName))
                .GroupBy(a => a.CategoryName)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                }).ToList();

            PieChartDataJson = JsonConvert.SerializeObject(pieChartData);
        }
    }
}