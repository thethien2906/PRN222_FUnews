﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using DataAccessObjects.Helper;
using Microsoft.EntityFrameworkCore;

    namespace DataAccessObjects
    {
        public class TagManager
        {
            private static TagManager instance = null;
            private static readonly object instanceLock = new object();
            private TagManager() { }
            public static TagManager Instance
            {
                get
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new TagManager();
                        }
                        return instance;
                    }
                }
            }
            public IEnumerable<Tag> GetTags()
            {
                List<Tag> tags = new List<Tag>();
                try
                {
                    using var _context = new FunewsManagementContext();
                    tags = _context.Tags.Include(a => a.NewsArticles).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return tags;
            }
            public Tag GetTagById(int id)
            {
                Tag tag = null;
                try
                {
                    using var _context = new FunewsManagementContext();
                    tag = _context.Tags.Include(a=>a.NewsArticles).SingleOrDefault(tag => tag.TagId == id);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return tag;
            }
            public void AddNew(Tag tag)
            {
                try
                {
                    using var _context = new FunewsManagementContext();

                    _context.Tags.Add(tag);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Error while deleting news article. Check inner exception for details.", ex);
                }
            }
            public void Update(Tag tag)
            {
                Tag existingTag = GetTagById(tag.TagId);
                try
                {
                    if (existingTag != null)
                    {
                        using var _context = new FunewsManagementContext();
                        _context.Entry(tag).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("The tag does not exist");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            
            //public serviceresponse delete(tag tag)
            //{
            //    var response = new serviceresponse();
            //    try
            //    {
            //        using var _context = new funewsmanagementcontext();

            //        if (tag.newsarticles.count > 0)
            //        {
            //            response.issuccess = false;
            //            response.message = "the tag cannot be deleted because it is associated with news articles.";
            //            return response;
            //        }
            //        _context.tags.remove(tag);
            //        _context.savechanges();

            //        response.issuccess = true;
            //        response.message = "tag deleted successfully.";
            //    }
            //    catch (dbupdateexception ex)
            //    {
            //        response.issuccess = false;
            //        response.message = "error while deleting tag. please try again later.";
            //    }

            //    return response;
            //}
            //list all tags that have the same news article
            public IEnumerable<Tag> GetTagsByNewsArticle(string newsArticleId)
            {
                List<Tag> tags = new List<Tag>();
                try
                {
                    using var _context = new FunewsManagementContext();
                    tags = _context.Tags
                        .Include(t => t.NewsArticles)
                        .Where(t => t.NewsArticles.Any(n => n.NewsArticleId == newsArticleId))
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return tags;
            }
        // get tags by ids
            public IEnumerable<Tag> GetTagsByIds(List<int> tagIds)
        {
            List<Tag> tags = new List<Tag>();
            try
            {
                using var _context = new FunewsManagementContext();
                tags = _context.Tags
                    .Include(t => t.NewsArticles)
                    .Where(t => tagIds.Contains(t.TagId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tags;
        }
            public void Delete(Tag tag)
        {
            try
            {
                using var _context = new FunewsManagementContext();
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error while deleting tag. Check inner exception for details.", ex);
            }
        }
  
        }
    }
