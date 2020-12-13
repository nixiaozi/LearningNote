using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(@"http://www.taobao.com");
            
            // 添加自己的User Agent





        }
    }   
}
