using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public class PerformDatabaseOperations
    {
        public static async Task TestOperations()
        {
            using(var db=new BloggingContext())
            {
                db.Blogs.Add(new AsyncDemo.Blog
                {
                    Name = "Test Blog #" + (db.Blogs.Count() + 1)
                });

                Console.WriteLine("Calling SaveChanges.");

                //await db.SaveChangesAsync();
                db.SaveChanges();
                Console.WriteLine("SaveChanges completed.");

                Console.WriteLine("Executing query.");
                var blogs = await (from b in db.Blogs
                                   orderby b.Name
                                   select b).ToListAsync();

                // Write all blogs out to Console
                Console.WriteLine("Query completed with following results:");
                foreach (var blog in blogs)
                {
                    Console.WriteLine(" - " + blog.Name);
                }
                
            }

        }
    }
}
