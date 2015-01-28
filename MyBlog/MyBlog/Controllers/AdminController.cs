using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyBlog.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "User");
            }
            return View();
        }


        //public ActionResult Testing1(string test)
        //{
        //    string ConnectionString = ConnectionStrings.ConnectionString();
        //    string query = @"INSERT INTO myblog (test) VALUES (@test)";

        //    using (MySqlConnection cnn = new MySqlConnection(ConnectionString))
        //    {
        //        cnn.Open();
        //        using (var cmd = cnn.CreateCommand())
        //        {
        //            cmd.CommandText = query;
        //            cmd.Parameters.AddWithValue("test", test);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}


    }
}
