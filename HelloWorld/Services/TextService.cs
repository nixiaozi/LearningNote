using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld.Services
{
    public class TextService : ITextService
    {
        private string _text;

        public TextService(string text)
        {
            _text = text;
        }

        public string GetText()
        {
            return _text;
        }
    }
}
