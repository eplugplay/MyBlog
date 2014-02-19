using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog
{
    public static class ConnectionStrings
    {
        public static string ConnectionString()
        {
            return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MyBlogConnectionString"].ConnectionString;
        }
    }
}