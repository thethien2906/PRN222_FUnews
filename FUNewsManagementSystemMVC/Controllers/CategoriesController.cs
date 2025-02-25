using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using Services.DTOs;
using System.Threading.Tasks;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        // GET: Categories/Details/5
        public IActionResult Details(short? id)
        {
            if (id == null) return NotFound();
            var category = _categoryService.GetCategoryById(id.Value);
            return category == null ? NotFound() : View(category);
        }

        // GET: Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryDesciption");
            return PartialView("Create", new CategoryDTO());
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            _categoryService.SaveCategory(categoryDTO);
            return Json(new { success = true });
        }

        // GET: Categories/Edit/5
        [HttpGet]
        public IActionResult Edit(short? id)
        {
            if (id == null) return NotFound();

            var category = _categoryService.GetCategoryById(id.Value);
            if (category == null) return NotFound();

            ViewData["ParentCategoryId"] = new SelectList(
                _categoryService.GetCategories(),
                "CategoryId",
                "CategoryDesciption",
                category.ParentCategoryId
            );

            return PartialView("Edit", category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(short id, CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            _categoryService.UpdateCategory(categoryDTO);
            return Json(new { success = true });
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(short? id)
        {
            if (id == null) return NotFound();

            var category = _categoryService.GetCategoryById(id.Value);
            if (category == null) return NotFound();

            return PartialView("Delete", category); // Ensure correct view name
        }

        public class DeleteCategoryRequest
        {
            public short Id { get; set; }
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed([FromBody] DeleteCategoryRequest request)
        {
            try
            {
                // Log the incoming request
                Console.WriteLine($"Received delete request for ID: {request?.Id}");

                if (request == null || request.Id <= 0)
                {
                    return BadRequest(new { success = false, error = "Invalid ID received" });
                }

                var category = _categoryService.GetCategoryById(request.Id);
                if (category == null)
                {
                    return NotFound(new { success = false, error = "Category not found" });
                }

                _categoryService.DeleteCategory(request.Id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting category: {ex.Message}");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}