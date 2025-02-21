using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Services.IService;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagService _contextTag;

        public TagsController(ITagService contextTag)
        {
            _contextTag = contextTag;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // Redirect to the login page or display an error message
                return RedirectToAction("Login", "SystemAccounts");
            }
            var tags = _contextTag.GetAllTags();
            // Load associated news articles for each tag
            foreach (var tag in tags)
            {
                tag.NewsArticles = _contextTag.GetTagsByArticleId(tag.TagId.ToString())
                    .SelectMany(t => t.NewsArticles)
                    .Distinct()
                    .ToList();
            }
            return View(tags);
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _contextTag.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Tag tag)
        {
            ModelState.Remove("TagId");
            if (ModelState.IsValid)
            { 
                try
                {
                    // Tag id auto increment
                    var lastTag = _contextTag.GetAllTags().OrderByDescending(n => n.TagId).FirstOrDefault();
                    int newId = lastTag != null ? lastTag.TagId + 1 : 1;
                    tag.TagId = newId;
                    _contextTag.AddTag(tag);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                } 
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _contextTag.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagId,TagName,Note")] Tag tag)
        {
            if (id != tag.TagId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contextTag.UpdateTag(tag);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.TagId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = _contextTag.GetTagById(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = _contextTag.GetTagById(id);
            if (tag == null)
            {
                TempData["ErrorMessage"] = "Tag not found.";
                return RedirectToAction(nameof(Index));
            }

            var response = _contextTag.DeleteTag(tag);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction("Delete", "Tags");
        }


        private bool TagExists(int id)
        {
            return _contextTag.GetTagById(id) != null;
        }
    }
}
