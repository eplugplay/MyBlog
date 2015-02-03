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
        private IEnumerable<playground> _playground = new List<playground>();
        //
        // GET: /PlayGround/

        public ActionResult Index()
        {
            _playground = from p in suMain.playgrounds
                          orderby p.id ascending
                          select p;
            return View(_playground.ToList());
        }

        public ActionResult Edit(int id)
        {
            playground editObject = (from p in suMain.playgrounds
                                     where p.id == id
                                     select p).FirstOrDefault();
            ViewBag.id = id;
            return View(editObject);
        }


        public ContentResult Update(int id, string text)
        {
            using (var context = new db_9ae46c_myblogEntities())
            {
                playground _se = context.playgrounds.FirstOrDefault(m => m.id == id);
                try
                {
                    _se.text = text;
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
            return Content("Success");
        }

        public ContentResult New(string text)
        {
            using (var context = new db_9ae46c_myblogEntities())
            {
                try
                {
                    playground insert = new playground()
                    {
                        text = text
                    };
                    context.playgrounds.AddObject(insert);
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
            return Content("Success");
        }

        public ContentResult Delete(int id, bool isLastRowDeleted)
        {
            using (var context = new db_9ae46c_myblogEntities())
            {
                var delete = (from s in suMain.playgrounds
                           where s.id == id
                           select s).FirstOrDefault();
                playground del = context.playgrounds.FirstOrDefault(m => m.id == id);
                try
                {
                    // delete row
                    context.playgrounds.DeleteObject(del);
                    context.SaveChanges();

                    // save row if last row
                    if (isLastRowDeleted)
                    {
                        New("");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return Content("Success");
        }

        [HttpPost]
        public ActionResult ReloadEditPartial()
        {
            _playground = from p in suMain.playgrounds
                          orderby p.id ascending
                           select p;
            return PartialView("_PlayGroundPartial", _playground.ToList());
        }

        public ContentResult GetRowCount()
        {
            var count = (from s in suMain.playgrounds
                         select s).Count();
            try
            {

            }
            catch (Exception e)
            {

            }
            return Content(count.ToString());
        }
    }
}
