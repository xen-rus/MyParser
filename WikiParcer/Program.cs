using System;
using WikiParcer.Classes;
using WikiParcer.Interfaces;

using System.Threading;
using System.Threading.Tasks;

namespace WikiParcer
{
    class Program
    {
        static object _lock = new object();
        static void Main(string[] args)
        {
            const string WikiLink = "http://www.wikipedia.org";

            IParser WikiParcer = new BaseParser(WikiLink);
                        
            WikiParcer.ParceAsync();

            Console.WriteLine($"Link =" + WikiLink + "wodrs count " + WikiParcer.GetWordCount());

            Parallel.ForEach(WikiParcer.GetChaildLinks(), (link) => {

                IParser ChildWiiParcer = new BaseParser(link);

                ChildWiiParcer.ParceAsync();

                Console.WriteLine($"Link =" + link + "wodrs count " + ChildWiiParcer.GetWordCount());
                

            });
        
        }
    }
}
