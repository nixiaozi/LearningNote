using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LeoProxyTest
{
    // 这是一个 Proxy 代理的实现类
    public class LeoProxy : IProxy
    {
        private HttpListener listener = new HttpListener();

        public void Create()
        {
            throw new NotImplementedException();
        }
    }

}
