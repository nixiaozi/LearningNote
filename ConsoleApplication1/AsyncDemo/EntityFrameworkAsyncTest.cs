using AsyncDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public static class EntityFrameworkAsyncTest
    {
        public static void TestMutiLineInsert()
        {
            List<Blog> blogs = new List<Blog>();
            using(var db=new BloggingContext())
            {
                var count = db.Blogs.Count();
                for(var i = 1; i < 200; i++)
                {
                    blogs.Add(new Blog
                    {
                        Name= "Test Blog #" + (count + i)
                    });
                }

                db.Blogs.AddRange(blogs);
                db.SaveChanges();
            }

        }

    }
}
