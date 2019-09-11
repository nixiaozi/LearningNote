using EntityFrameworkTest.Context;
using EntityFrameworkTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTest
{
    public class EntityFrameworkTestHelper
    {
        public static string GetRandomMemberName()
        {
            string[] nameArray =new string[] { "Jim","Mei","Lily","Sea","Tea","Smith","Dout","Gdge","SunShine","Holiday"};

            Random random = new Random();
            var r = random.Next(9);
            return nameArray[r];
        }

        public static int GetRandomAge()
        {
            Random random = new Random();
            var r = random.Next(80)+5;
            return r;
        }


        public static string RandomWorkLocation()
        {
            string[] nameArray = new string[] { "HongKong", "ShangHai", "NewYork", "London", "Roman", "Pairs", "Hangzhou", "Guangzhou", "Tibet", "Tokyo" };

            Random random = new Random();
            var r = random.Next(9);
            return nameArray[r];
        }


        public static string RandomWorkName()
        {
            string[] nameArray = new string[] { "Image", "Word", "PPT", "Excel", "PDF", "Speak", "Tell", "Walk", "Run", "Jump" };

            Random random = new Random();
            var r = random.Next(9);
            return nameArray[r];
        }





        public static async void InitLoadData()
        {
            using (var context = new EFDbContext())
            {
                for (var i = 0; i < 1000; i++)
                {
                    var theMember = new Member
                    {
                        ID = Guid.NewGuid(),
                        Age = EntityFrameworkTestHelper.GetRandomAge(),
                        CreateDate = DateTime.Now.AddDays(EntityFrameworkTestHelper.GetRandomAge()),
                        Name = EntityFrameworkTestHelper.GetRandomMemberName(),
                    };
                    context.Members.Add(theMember);
                    Console.WriteLine("插入Member数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(theMember));

                    Random r = new Random();
                    for (int worknum = r.Next(12) + 1; worknum > 0; worknum--)
                    {
                        var theWork = new Work
                        {
                            ID = Guid.NewGuid(),
                            MemberId = theMember.ID,
                            WorkLocation = EntityFrameworkTestHelper.RandomWorkLocation(),
                            WorkName = EntityFrameworkTestHelper.RandomWorkName(),
                            WorkTimes = r.Next(6) + 1,
                        };

                        context.Works.Add(theWork);
                        Console.WriteLine("插入Work数据：" + Newtonsoft.Json.JsonConvert.SerializeObject(theWork));
                    }
                }

                await context.SaveChangesAsync();
            }

        }

    }
}
