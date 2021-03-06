﻿using BlogsConsole.Models;
using System;
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


            try
            {

                do
                {

                    Console.WriteLine("----------------------------");
                    Console.WriteLine();
                    Console.WriteLine("Enter 1 to ADD BLOG");
                    Console.WriteLine("Enter 2 to VIEW ALL BLOGS");
                    Console.WriteLine("Enter 3 to ADD POST TO BLOG");
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

                            Console.WriteLine("ENTER THE NAME OF BLOG TO ADD POST TO: ");
                            var blogName = Console.ReadLine().ToLower();

                            var blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));

                            bool blogExists;

                            blogExists = blogQuery.Any() ? true : false;

                            var blogID = 0;



                            if (blogExists)
                            {

                                foreach (Blog b in blogQuery)
                                {
                                    blogID = b.BlogId;
                                }

                                Console.WriteLine("Enter Post Title");
                                var title = Console.ReadLine().ToLower();

                                // handles title being null
                                while (title == null)
                                {
                                    logger.Warn("POST TITLE WAS NULL");

                                    Console.WriteLine("Title cannot be blank, Re-enter Post Title: ");
                                    title = Console.ReadLine();
                                }

                                Console.WriteLine("Type your Post");
                                var content = Console.ReadLine();

                                // handles content being empty
                                while (content == null)
                                {
                                    logger.Warn("POST CONTENT WAS EMPTY");

                                    Console.WriteLine("Content cannot be empty, Re-type Post: ");
                                    content = Console.ReadLine();
                                }

                                var post = new Post { Title = title, Content = content, BlogId = blogID };

                                db.AddPost(post);

                                logger.Info("Post Added - {title}", title);

                            }
                            else

                            {
                                // handles user entered blog title not existing in the database
                                string resp = null;

                                while (resp != "exit" && blogExists == false)
                                {
                                    Console.WriteLine("There is no blog with that name");
                                    blogExists = false;
                                    Console.WriteLine("Re-Enter the name of the blog or type EXIT to return to menu");
                                    resp = Console.ReadLine().ToLower();
                                    if (resp != "exit")
                                    {
                                        blogQuery = db.Blogs.Where(b => b.Name.Equals(blogName));
                                        blogExists = blogQuery.Any() ? true : false;

                                        if (blogExists == true)
                                        {
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
