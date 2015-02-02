using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class PlayGroundController : Controller
    {
        private Models.db_9ae46c_myblogEntities suMain = new db_9ae46c_myblogEntities();
        private IEnumerable<systemuser> _systemUser = new List<systemuser>();
        //
        // GET: /PlayGround/

        public ActionResult Index()
        {
            _systemUser = from p in suMain.systemusers
                          where p.Email != "steveyi32@gmail.com"
                          select p;
            return View(_systemUser.ToList());
        }

        public ActionResult Edit(int id)
        {
            systemuser editObject = (from p in suMain.systemusers
                                     where p.id == id
                                     select p).FirstOrDefault();
            ViewBag.id = id;
            return View(editObject);
        }


        public ContentResult Save(int id, string email)
        {
            using (var context = new db_9ae46c_myblogEntities())
            {
                systemuser _se = context.systemusers.FirstOrDefault(m => m.id == id);
                try
                {
                    _se.Email = email;
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
            //_systemUser = from p in suMain.systemusers
            //              select p;
            //return View(_systemUser.ToList());
            return Content("Success");
        }
    }
}
