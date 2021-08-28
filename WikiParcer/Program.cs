using System;
using WikiParcer.Classes;
using WikiParcer.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using WikiParser.Interfaces;
using WikiParser.Classes;

namespace WikiParcer
{
    class Program
    {

        static  void Main(string[] args)
        {
            const string WikiLink = "http://www.wikipedia.org";

            IParser wikiParser = new BaseParser(WikiLink);

            wikiParser.Connect();
            wikiParser.ParceAsync();

            Console.WriteLine($"Link =" + WikiLink + " words count " + wikiParser.GetWordCount());

         //   IThreadWorker parralelWorker = new ParallelWorker(WikiParcer.GetChaildLinks()); // It is better
            IThreadWorker parralelWorker = new TaskWorker(wikiParser.GetChaildLinks());

            parralelWorker.Run();
        }
    }
}
