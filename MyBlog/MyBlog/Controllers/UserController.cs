using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyBlog.Core;
using MyBlog.Models;
using Newtonsoft.Json;
using System.Text;

namespace MyBlog.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

           private readonly IBlogRepository _blogRepository;

        public UserController(IBlogRepository blogRepository = null)
        {
            _blogRepository = blogRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Email, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    Session["IsLoggedIn"] = "true";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email Address or Password is Incorrect.");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Models.db_9ae46c_myblogEntities())
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var encrpPass = crypto.Compute(user.Password);
                    var sysUser = db.systemusers.CreateObject();

                    sysUser.Email = user.Email;
                    sysUser.Password = encrpPass;
                    sysUser.PasswordSalt = crypto.Salt;
                    sysUser.UserID = Guid.NewGuid().ToString();
                    db.systemusers.AddObject(sysUser);
                    if (db.systemusers.Any(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Email Already Exists");
                        return View(user);
                    }
                    db.SaveChanges();
                }
            }
                return RedirectToAction("LogIn", "User");
        }


        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            // Clear authentication cookie
            HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            rFormsCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(rFormsCookie);
            // Clear session cookie 
            HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
            rSessionCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(rSessionCookie);
            Session["IsLoggedIn"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOut(Models.UserModel user)
        {
            return View();
        }

        private bool IsValid(string email, string password)
        {
            bool isValid = false;
            var crypto = new SimpleCrypto.PBKDF2();
            using (var db = new Models.db_9ae46c_myblogEntities())
            {
                var user = db.systemusers.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        isValid = true;
                    }
                }

            }
            return isValid;
        }


        /*admin tab for post grid*/
        public ContentResult Posts(JqInViewModel jqParams)
        {
            var posts = _blogRepository.Posts(jqParams.page - 1, jqParams.rows,
                jqParams.sidx, jqParams.sort == "asc");

            var totalPosts = _blogRepository.TotalPosts(false);

            return Content(JsonConvert.SerializeObject(new
            {
                page = jqParams.page,
                records = totalPosts,
                rows = posts,
                total = Math.Ceiling(Convert.ToDouble(totalPosts) / jqParams.rows)
            }), "application/json");
        }


        // add post 
        [HttpPost, ValidateInput(false)]
        public ContentResult AddPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (TryValidateModel(post))
            {
                var id = _blogRepository.AddPost(post);

                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Post added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the post."
                });
            }

            return Content(json, "application/json");
        }

        public ContentResult GetCategoriesHtml()
        {
            var categories = _blogRepository.Categories().OrderBy(s => s.Name);

            var sb = new StringBuilder();
            sb.AppendLine(@"<select>");

            foreach (var category in categories)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>",
                    category.Id, category.Name));
            }

            sb.AppendLine("<select>");
            return Content(sb.ToString(), "text/html");
        }

        public ContentResult GetTagsHtml()
        {
            var tags = _blogRepository.Tags().OrderBy(s => s.Name);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<select multiple=""multiple"">");

            foreach (var tag in tags)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>",
                    tag.Id, tag.Name));
            }

            sb.AppendLine("<select>");
            return Content(sb.ToString(), "text/html");
        }

        // edit post
        [HttpPost, ValidateInput(false)]
        public ContentResult EditPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (TryValidateModel(post))
            {
                _blogRepository.EditPost(post);
                json = JsonConvert.SerializeObject(new
                {
                    id = post.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        // delete post
        [HttpPost]
        public ContentResult DeletePost(int id)
        {
            _blogRepository.DeletePost(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Post deleted successfully."
            });

            return Content(json, "application/json");
        }

        public ContentResult Categories()
        {
            var categories = _blogRepository.Categories();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = categories.Count,
                rows = categories,
                total = 1
            }), "application/json");
        }

        // add category
        [HttpPost]
        public ContentResult AddCategory([Bind(Exclude = "Id")]Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the category."
                });
            }

            return Content(json, "application/json");
        }

        // edit category
        [HttpPost]
        public ContentResult EditCategory(Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = category.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        // delete category
        [HttpPost]
        public ContentResult DeleteCategory(int id)
        {
            _blogRepository.DeleteCategory(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json");
        }

        // tags
        public ContentResult Tags()
        {
            var tags = _blogRepository.Tags();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count,
                rows = tags,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddTag([Bind(Exclude = "Id")]Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Tag added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the tag."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            _blogRepository.DeleteTag(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Tag deleted successfully."
            });

            return Content(json, "application/json");
        }

    }
}
