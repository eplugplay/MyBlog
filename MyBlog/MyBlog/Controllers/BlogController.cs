﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Core;
using MyBlog.Models;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using Newtonsoft.Json;

namespace MyBlog.Views
{
    public class BlogController : Controller
    {
        //
        // GET: /Blog/

        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, p);
            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""",
                viewModel.Category.Name);
            return View("List", viewModel);
        }

        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""",
                viewModel.Tag.Name);
            return View("List", viewModel);
        }

        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found
                        for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

        // individual post
        public ViewResult Post(int year, int month, string title, int id = 0)
        {
            var post = (MyBlog.Core.Post)null;

            if (id == 0)
            {
                post = _blogRepository.Post(year, month, title);
            }
            else if (id != 0)
            {
                post = _blogRepository.GoToPost(id);
            }

            if (post == null)
                throw new HttpException(404, "Post not found");

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }

        // side bar
        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(_blogRepository);
            return PartialView("_Sidebars", widgetViewModel);
        }

    }
}
