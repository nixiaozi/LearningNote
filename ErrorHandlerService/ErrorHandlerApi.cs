using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorHandlerService
{
    public static class ErrorHandlerApi
    {
        //
        public static K DoApi<T,K>(T Data,Func<T,K> DoSomething,Func<T,bool> DoSuccess,Func<T,bool> DoError,Func<T,bool> DoFinish)
        {
            K result;
            try
            {


                result = DoSomething(Data);
                return result;
            }
            catch(Exception e)
            {
                DoError(Data);
                throw e;
            }
            finally
            {
                DoFinish(Data);
            }
            

        }



    }
}
