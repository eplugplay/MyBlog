using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyBlog.Core;

namespace MyBlog.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository blogRepository, int p)
        {
            Posts = blogRepository.Posts(p - 1, 10);
            TotalPosts = blogRepository.TotalPosts();
            PostType = "Default";
        }


        public ListViewModel(IBlogRepository blogRepository,
            string categorySlug, int p)
        {
            Posts = blogRepository.PostsForCategory(categorySlug, p - 1, 10);
            TotalPosts = blogRepository.TotalPostsForCategory(categorySlug);
            Category = blogRepository.Category(categorySlug);
            PostType = "Category";
        }

        public ListViewModel(IBlogRepository blogRepository,
        string text, string type, int p)
        {
            switch (type)
            {
                case "Category":
                    Posts = blogRepository.PostsForCategory(text, p - 1, 10);
                    TotalPosts = blogRepository.TotalPostsForCategory(text);
                    Category = blogRepository.Category(text);
                    PostType = "Category";
                    break;
                case "Tag":
                    Posts = blogRepository.PostsForTag(text, p - 1, 10);
                    TotalPosts = blogRepository.TotalPostsForTag(text);
                    Tag = blogRepository.Tag(text);
                    PostType = "Tag";
                    break;
                default:
                    Posts = blogRepository.PostsForSearch(text, p - 1, 10);
                    TotalPosts = blogRepository.TotalPostsForSearch(text);
                    Search = text;
                    PostType = "Default";
                    break;
            }
        }

        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public Category Category { get; private set; }
        public string PostType { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }
    }
}