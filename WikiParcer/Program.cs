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

            IParser WikiParcer = new BaseParser(WikiLink);

            WikiParcer.Connect();
            WikiParcer.ParceAsync();

            Console.WriteLine($"Link =" + WikiLink + " words count " + WikiParcer.GetWordCount());

         //   IThreadWorker parralelWorker = new ParallelWorker(WikiParcer.GetChaildLinks()); // It is better
            IThreadWorker parralelWorker = new TaskWorker(WikiParcer.GetChaildLinks());

            parralelWorker.Run();


            
            /*
            ParallelOptions parallelOptions = new ParallelOptions();

            var Proccessor = Environment.ProcessorCount;

            parallelOptions.MaxDegreeOfParallelism = Proccessor;

            Parallel.ForEach(WikiParcer.GetChaildLinks(), parallelOptions, (link) => {

                var ChildWikiParcer = new BaseParser(link);
                ChildWikiParcer.Connect().Wait(200);
                ChildWikiParcer.ParceAsync();

                Console.WriteLine($"Link =" + link + " has " + ChildWikiParcer.GetWordCount() + " words.");
         
           }); */
        
        }
    }
}
