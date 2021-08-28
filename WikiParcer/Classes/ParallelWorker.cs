using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiParcer.Classes;
using WikiParser.Interfaces;

namespace WikiParser.Classes
{
    class ParallelWorker : IThreadWorker
    {
        static object _lock = new object();
        int counter;
        List<string> linkList;

        public ParallelWorker(List<string> links)
        {
            linkList = links;

        }

        public void Run()
        {
            ParallelOptions parallelOptions = new ParallelOptions();

            var Proccessor = Environment.ProcessorCount;

            parallelOptions.MaxDegreeOfParallelism = Proccessor;

            Parallel.ForEach(linkList, parallelOptions, (link,loop) => { //Here we can use parral.For

                lock (_lock)
                    if (counter >= 10)
                            loop.Break();

                var ChildWikiParcer = new BaseParser(link);
                 ChildWikiParcer.Connect().Wait(200);
                ChildWikiParcer.ParceAsync();

                Console.WriteLine($"Link =" + link + " has " + ChildWikiParcer.GetWordCount() + " words.");

                lock (_lock)
                    counter++;
            });
        }
    }
}
