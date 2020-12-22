using BenderProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumTest
{
    public static class SeleniumProxyTest
    {
        public static void OnResponseReceived(ProcessingContext context)
        {
            Console.WriteLine("注意，收到一个服务端响应 url==>" + context.ResponseHeader.Server);
            context.StopProcessing();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void OnResponseSent(ProcessingContext context)
        {
            Console.WriteLine("注意，发送一个响应到客户端！");
            context.StopProcessing();
        }

        public static void OnRequestReceived(ProcessingContext context)
        {
            Console.WriteLine("注意，收到一个客户端的请求！");
            context.StopProcessing();
        }


        /// 没有OnRequestSend

    }

}
