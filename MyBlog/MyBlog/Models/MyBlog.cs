using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class MyBlog
    {
        public string test { get; set; }

        // image
        public string ImagePath
        {
            get
            {
                return "/Images/steveyi.jpg";
            }
        }
    }
}