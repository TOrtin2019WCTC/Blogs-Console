using BlogsConsole.Models;
using System;
using System.Linq;


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


            try
            {

                do
                {

                    Console.WriteLine("----------------------------");
                    Console.WriteLine("\nEnter 1 to ADD BLOG");
                    Console.WriteLine("Enter 2 to VIEW ALL BLOGS");
                    Console.WriteLine("Enter 3 to ADD POST TO BLOG");
                    Console.WriteLine("Enter QUIT to exit");
                    Console.WriteLine("----------------------------");

                    ans = Console.ReadLine();

                    switch (ans)
                    {
                        case "1":

                            logger.Info("Choice: Create Blog");
                            // Create and save a new Blog
                            Console.Write("Enter a name for a new Blog: ");
                            var name = Console.ReadLine().ToLower();

                            var blog = new Blog { Name = name };


                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);

                            break;

                        case "2":

                            logger.Info("Choice: View All Blogs");
                            // Display all Blogs from the database
                            var query = db.Blogs.OrderBy(b => b.Name);

                            Console.WriteLine("All blogs in the database:");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                            }

                            break;

                        case "3":

                            // Create and save new Post
                            logger.Info("Choice: Create New Post");

                            Console.WriteLine("Enter the name of the blog to add post to");
                            var blogName = Console.ReadLine().ToLower();

                            var blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));
                            var blogID = 0;
                            bool blogExists = false;
                            string resp = null;

                            while (blogExists == false || resp != "exit")
                            {

                                if (blogQuery != null)
                                {
                                    blogExists = true;
                                    foreach (Blog b in blogQuery)
                                    {
                                        blogID = b.BlogId;
                                    }

                                    Console.WriteLine("Enter Post Title");
                                    var title = Console.ReadLine().ToLower();

                                    Console.WriteLine("Type your Post");
                                    var content = Console.ReadLine();

                                    var post = new Post { Title = title, Content = content, BlogId = blogID };

                                    db.AddPost(post);

                                    logger.Info("Post Added - {title}", title);

                                }
                                else
                                {
                                    Console.WriteLine("There is no blog with that name");
                                    blogExists = false;
                                    Console.WriteLine("ReEnter the name of the blog or type EXIT to quit");
                                    resp = Console.ReadLine().ToLower();
                                    if (resp != "exit")
                                    {
                                        blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));
                                    }

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
