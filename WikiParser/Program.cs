using System;
using WikiParcer.Classes;
using WikiParcer.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using WikiParser.Interfaces;
using WikiParser.Classes;
using System.Diagnostics;

namespace WikiParcer
{
    class Program
    {
        static  void Main(string[] args)
        {
            const string WikiLink = "http://www.wikipedia.org";

            IParser wikiParser = new BaseParser(WikiLink);

            wikiParser.Connect();
            wikiParser.Parce();

            Console.WriteLine($"Link =" + WikiLink + " words count " + wikiParser.GetWordCount());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //parallerFor
            IThreadWorker parallelWorker = new ParallelWorker(wikiParser.GetChaildLinks()); // It is better

            parallelWorker.Run();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();


            //Task Realization
            parallelWorker = new TaskWorker(wikiParser.GetChaildLinks());
           
            parallelWorker.Run();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}
