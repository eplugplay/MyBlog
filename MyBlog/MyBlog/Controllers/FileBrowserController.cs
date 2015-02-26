using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class FileBrowserController : Controller
    {
        //
        // GET: /FileBrowser/

        public ActionResult Index()
        {
            return View();
        }

        public string GetURL()
        {
            string url = "http://www.steve-yi.com/images/";
            return url;
        }

        [HttpPost]
        public ActionResult GetImages()
        {
            var directory = new System.IO.DirectoryInfo(Server.MapPath("/Images/"));
            string html = "<table>";
            int i = 0;
            foreach (var file in directory.GetFilesByExtensions(".png", ".gif", ".jpg"))
            {
                if (i % 20 == 0)
                {
                    html += "<tr><td><div class=\"thumbnail\"><img style=\"height:80px;width:80px;\" src=\"" + GetURL() + file.Name + "\"" + " alt=\"thumb\" title=\"" + GetURL() + file.Name + "\"/></div></td>";
                }
                else
                {
                    html += "<td><div class=\"thumbnail\"><img style=\"height:80px;width:80px;\" src=\"" + GetURL() + file.Name + "\"" + " alt=\"thumb\" title=\"" + GetURL() + file.Name + "\"/></div></td>";
                }

                if (i == file.Length - 1)
                {
                    html += "</table>";
                }
                i++;
            }

            return Content(html);
        }

        [HttpPost]
        public JsonResult DeleteImage(string fileName)
        {
            string path = Server.MapPath("/Images/") + fileName;
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception e)
            {
                return Json("Failed to delete.");
            }
            return Json("Successfully deleted.");
        }

        [HttpPost]
        public JsonResult UploadImage()
        {
            string fileName = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        fileName = fileContent.FileName;
                        string[] split = fileName.Split('\\');
                        fileName = split[split.Length - 1];
                        var stream = fileContent.InputStream;
                        using (var fileStream = System.IO.File.Create(Server.MapPath("/Images/") + fileName))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json("Upload failed");
            }

            return Json("Successful");
        }
    }
}
