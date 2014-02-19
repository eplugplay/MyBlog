using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        MyBlog.Models.MyBlog _myblog = new Models.MyBlog();
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to my blog.";
            //_myblog.test = "finally!";
            //return View(_myblog);
            return View();
        }


        public ActionResult About()
        {
            
            return View();
        }

        public ActionResult Interests()
        {
            return View();
        }
    }
}
