using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld.Services
{
    public class WelcomeService : IWelcomeService
    {
        public string DoWelocme()
        {
            return "DoWelocme";
        }

        public string SayWelcome()
        {
            return "SayWelcome";
        }
    }
}
