using System;
using System.Collections.Generic;
using System.Text;

namespace LeoProxyTest
{
    public class ProxyFactory<T> where T:IProxy
    {
        public virtual void Init(T target)
        {
            target.Create();
        }

        

    }

}
