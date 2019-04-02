using BlogsConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/*
   For testing purposes, the blog names in my database are:
   
   hi
   est
   working?
   snowflake tears
   
   
*/
namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string ans;
            var db = new BloggingContext();
            List<int> blogIds;


            try
            {

                do
                {

                    Console.WriteLine("----------------------------");
                    Console.WriteLine();
                    Console.WriteLine("Enter 1 to ADD BLOG");
                    Console.WriteLine("Enter 2 to DISPLAY ALL BLOGS");
                    Console.WriteLine("Enter 3 to CREATE NEW POST");
                    Console.WriteLine("Enter 4 to DISPLAY ALL POSTS");
                    Console.WriteLine("Enter QUIT to exit");
                    Console.WriteLine();
                    Console.WriteLine("----------------------------");

                    ans = Console.ReadLine();

                    switch (ans)
                    {
                        case "1":

                            logger.Info("Choice: Create Blog");
                            // Create and save a new Blog
                            Console.Write("Enter a name for a new Blog: ");
                            var name = Console.ReadLine().ToLower();

                            if (name != "")
                            {
                                var blog = new Blog { Name = name };
                                db.AddBlog(blog);
                                logger.Info("Blog added - {name}", name);
                            }
                            else
                            {
                                logger.Error("Blog name cannot be null");
                            }



                            break;

                        case "2":

                            logger.Info("Choice: Display all blogs");
                            // Display all Blogs from the database
                            var query = db.Blogs.OrderBy(b => b.Name);
                            var blogsReturned = query.Count();

                            Console.WriteLine($"{blogsReturned} Blogs returned");
                            Console.WriteLine();
                            //Console.WriteLine("All blogs in the database:");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }

                            break;

                        case "3":

                            // Create and save new Post
                            logger.Info("Choice: Create New Post");

                            var blogListQuery = db.Blogs.OrderBy(b => b.BlogId);
                            blogIds = new List<int>();
                            string choice;

                            if (blogListQuery.Count() > 0)
                            {

                                Console.WriteLine();
                                Console.WriteLine("Select the blog you would like to post to:");
                                foreach (var item in blogListQuery)
                                {
                                    Console.WriteLine($"{item.BlogId}) {item.Name}");
                                    blogIds.Add(item.BlogId);
                                }


                                choice = Console.ReadLine();

                                int blogChoice = 0;

                                try
                                {
                                    blogChoice = int.Parse(choice);

                                    if (blogIds.Contains(blogChoice))
                                    {
                                        Console.WriteLine("Enter Post Title");
                                        var title = Console.ReadLine().ToLower();

                                        // handles title being null
                                        while (title == null)
                                        {
                                            logger.Error("POST TITLE CANNOT BE NULL");

                                            Console.WriteLine("Title cannot be blank, Re-enter Post Title: ");
                                            title = Console.ReadLine();
                                        }

                                        Console.WriteLine("Type your Post");
                                        var content = Console.ReadLine();

                                        // handles content being empty
                                        while (content == null)
                                        {
                                            logger.Error("POST CONTENT CANNOT BE NULL");

                                            Console.WriteLine("Content cannot be empty, Re-type Post: ");
                                            content = Console.ReadLine();
                                        }

                                        var post = new Post { Title = title, Content = content, BlogId = blogChoice };

                                        db.AddPost(post);

                                        logger.Info("Post Added - {title}", title);


                                    }
                                    else
                                    {
                                        logger.Error("There are no Blogs saved with that ID");
                                    }
                                }
                                catch (Exception)
                                {
                                    logger.Error("Invalid Blog Id");
                                }


                            }
                            else
                            {
                                logger.Error("There are no blogs to write a post to");
                            }


                            //var blogToPostTo;




                            //Console.WriteLine("ENTER THE NAME OF BLOG TO ADD POST TO: ");
                            //var blogName = Console.ReadLine().ToLower();

                            //var blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));

                            //bool blogExists;

                            //blogExists = blogQuery.Any() ? true : false;

                            //var blogID = 0;



                            //if (blogExists)
                            //{

                            //    foreach (Blog b in blogQuery)
                            //    {
                            //        blogID = b.BlogId;
                            //    }

                            //    Console.WriteLine("Enter Post Title");
                            //    var title = Console.ReadLine().ToLower();

                            //    // handles title being null
                            //    while (title == null)
                            //    {
                            //        logger.Warn("POST TITLE WAS NULL");

                            //        Console.WriteLine("Title cannot be blank, Re-enter Post Title: ");
                            //        title = Console.ReadLine();
                            //    }

                            //    Console.WriteLine("Type your Post");
                            //    var content = Console.ReadLine();

                            //    // handles content being empty
                            //    while (content == null)
                            //    {
                            //        logger.Warn("POST CONTENT WAS EMPTY");

                            //        Console.WriteLine("Content cannot be empty, Re-type Post: ");
                            //        content = Console.ReadLine();
                            //    }

                            //    var post = new Post { Title = title, Content = content, BlogId = blogID };

                            //    db.AddPost(post);

                            //    logger.Info("Post Added - {title}", title);

                            //}
                            //else

                            //{
                            //// handles user entered blog title not existing in the database
                            //string resp = null;

                            //while (resp != "exit" && blogExists == false)
                            //{
                            //    Console.WriteLine("There is no blog with that name");
                            //    blogExists = false;
                            //    Console.WriteLine("Re-Enter the name of the blog or type EXIT to return to menu");
                            //    resp = Console.ReadLine().ToLower();
                            //    if (resp != "exit")
                            //    {
                            //        blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));
                            //        blogExists = blogQuery.Any() ? true : false;

                            //        if (blogExists == true)
                            //        {
                            //            foreach (Blog b in blogQuery)
                            //            {
                            //                blogID = b.BlogId;
                            //            }

                            //            Console.WriteLine("Enter Post Title");
                            //            var title = Console.ReadLine().ToLower();

                            //            Console.WriteLine("Type your Post");
                            //            var content = Console.ReadLine();

                            //            var post = new Post { Title = title, Content = content, BlogId = blogID };

                            //            db.AddPost(post);

                            //            logger.Info("Post Added - {title}", title);
                            //        }
                            //    }
                            //}

                            //}
                            break;

                        case "4":

                            blogIds = new List<int>();
                            logger.Info("Choice: Display Posts");
                            Console.WriteLine("Select Blog's posts to display");
                            Console.WriteLine();
                            Console.WriteLine("0) Posts from all blogs");
                            var postDisplayQuery = db.Blogs.OrderBy(b => b.BlogId);


                            foreach (var blog in postDisplayQuery)
                            {
                                Console.WriteLine($"{blog.BlogId}) Posts from {blog.Name}");
                                blogIds.Add(blog.BlogId);


                            }

                            var displayPosts = db.Posts.OrderBy(p => p.PostId);

                            string resp = Console.ReadLine();
                            int blogId;

                            if (!resp.Equals("0") & !resp.Equals(null))
                            {
                                try
                                {
                                    blogId = Int32.Parse(resp);

                                    if (blogIds.Contains(blogId))
                                    {
                                        var getPostsFromSpecificBlog = (from p in db.Posts
                                                                        join b in db.Blogs
                                                                        on p.BlogId equals b.BlogId
                                                                        where p.BlogId == blogId
                                                                        select new
                                                                        {
                                                                            b.Name,
                                                                            p.Title,
                                                                            p.Content

                                                                        }).ToList();


                                        Console.WriteLine($"{getPostsFromSpecificBlog.Count()} Post(s) returned");
                                        foreach (var post in getPostsFromSpecificBlog)
                                        {
                                            Console.WriteLine($"Blog: {post.Name}");
                                            Console.WriteLine($"Title: {post.Title}");
                                            Console.WriteLine($"Content: {post.Content}");
                                            Console.WriteLine();
                                        }


                                    }
                                    else
                                    {
                                        logger.Error("Blog does not exist in the database");
                                    }


                                }
                                catch (FormatException)
                                {
                                    logger.Error("Invalid Blog ID");
                                }

                            }
                            else if (resp.Equals("0"))
                            {
                                Console.WriteLine($"{displayPosts.Count()} Post(s) returned");
                                Console.WriteLine();

                                var getAllBlogs = (from p in db.Posts
                                                   join b in db.Blogs
                                                   on p.BlogId equals b.BlogId
                                                   select new
                                                   {
                                                       b.Name,
                                                       p.Title,
                                                       p.Content

                                                   }).ToList();


                                foreach (var posts in getAllBlogs)
                                {
                                    Console.WriteLine($"Blog: {posts.Name}");
                                    Console.WriteLine($"Title: {posts.Title}");
                                    Console.WriteLine($"Content: {posts.Content}");
                                    Console.WriteLine();
                                }


                            }

                            break;


                    }





                } while (!ans.Equals("quit"));



            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
