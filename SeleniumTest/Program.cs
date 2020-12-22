using BenderProxy;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Drawing;

namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 添加一个代理服务器
            HttpProxyServer proxyServer = new HttpProxyServer("localhost", new HttpProxy(19991));
            proxyServer.Start().WaitOne();
            // proxyServer.Start();
            Console.WriteLine("Started on port {0}", proxyServer.ProxyEndPoint.Port);

            //proxyServer.Proxy.OnResponseSent = SeleniumProxyTest.OnResponseSent;
            //proxyServer.Proxy.OnResponseReceived = SeleniumProxyTest.OnResponseReceived;
            //proxyServer.Proxy.OnRequestReceived = SeleniumProxyTest.OnRequestReceived;


            // 添加自己的User Agent  https://stackoverflow.com/questions/29916054/change-user-agent-for-selenium-web-driver
            ChromeOptions chromeOptions = new ChromeOptions();
            Proxy proxy = new Proxy();
            proxy.HttpProxy = string.Format("{0}:{1}", "127.0.0.1", proxyServer.ProxyEndPoint.Port);
            chromeOptions.Proxy = proxy;   // 为Webdriver 添加代理来访问网站
            chromeOptions.UseSpecCompliantProtocol = true; // Force spec-compliant protocol dialect for now.
            //chromeOptions.AddArgument("user-agent=whatever you want"); 
            ///


            // chromeOptions.AddArgument("cookie=dgisagegihdsghruoghreghuerhguhddshgurguhrgfddfg"); // 并不是正确添加cookie的方法






            var driver = new ChromeDriver(chromeOptions);


            


            driver.Manage().Window.Size = new Size(1024, 768); // 这个是手动调整浏览器窗口的大小，小的浏览器窗口可能导致自动跳转移动端网站

            #region 基础的 webdriver 浏览操作
            /// 以下是基础的 webdriver 浏览操作
            driver.Navigate().GoToUrl(@"http://www.taobao.com"); // C# 不支持driver.get("https://selenium.dev"); 这种方式


            var currentUrl = driver.Url;
            Debug.Print("当前浏览器地址为" + currentUrl);// 这个地址可能是最后经过302重定向的地址

            // 第二跳到百度
            driver.Navigate().GoToUrl(@"http://www.baidu.com"); // 在这个地址后driver的地址也立刻改变了
            Debug.Print("当前浏览器地址为1==>" + driver.Url);

            //返回
            driver.Navigate().Back();
            Debug.Print("当前浏览器地址为2==>" + driver.Url);

            //向前 
            driver.Navigate().Forward();
            Debug.Print("当前浏览器地址为3==>" + driver.Url);

            //刷新当前页面
            driver.Navigate().Refresh();
            Debug.Print("刷新页面后浏览器地址==>" + driver.Url); // 这里是百度页面
            Debug.Print("当前页面标题为==>" + driver.Title);   // Debug.Fail 会导致调用中断


            //Store the ID of the original window
            string originalWindow = driver.CurrentWindowHandle;

            //Check we don't have other windows open already
            Assert.AreEqual(driver.WindowHandles.Count, 1);  //  Asset 是属于测试框架 Nunit中的配置

            //Click the link which opens in a new window
            driver.FindElement(By.LinkText("地图")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            Debug.Print("WebDriverWait进行了定义，当前时间为==>"+DateTime.Now.ToLongTimeString());

            //Wait for the new window or tab
            wait.Until(wd => wd.WindowHandles.Count == 2);

            //Loop through until we find a new window handle
            foreach (string window in driver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    driver.SwitchTo().Window(window);
                    break;
                }
            }
            //Wait for the new tab to finish loading content
            wait.Until(wd => wd.Title == "百度地图");
            Debug.Print("WebDriverWait等待了百度地图加载完毕，当前时间为==>" + DateTime.Now.ToLongTimeString());

            // 需要使用Selenium 4 和以后的版本才可以使用下面两个方法新建窗口和选项卡
            /*// Opens a new tab and switches to new tab
            driver.SwitchTo().NewWindow(WindowType.Tab);
            // Opens a new window and switches to new window
            driver.SwitchTo().NewWindow(WindowType.Window);*/

            #endregion

            #region Cookie相关操作
            var thecookie = new Cookie("aa", "bbbc");
            driver.Manage().Cookies.AddCookie(thecookie);
            driver.Manage().Cookies.AddCookie(new Cookie("bb","cccde"));
            var allCookies = driver.Manage().Cookies.AllCookies;
            driver.Manage().Cookies.DeleteAllCookies(); // 删除所有的Cookies

            #endregion




            driver.Quit();



        }


        
    }   


    
}
