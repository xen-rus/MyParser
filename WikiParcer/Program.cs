using System;
using WikiParcer.Classes;
using WikiParcer.Interfaces;

namespace WikiParcer
{
    class Program
    {
        static void Main(string[] args)
        {
            IParser WikiParcer = new BaseParser();

            WikiParcer.Route("http://www.wikipedia.org");

            WikiParcer.ParceAsync();
        }
    }
}
