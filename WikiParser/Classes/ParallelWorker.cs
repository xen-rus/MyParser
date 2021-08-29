﻿using System;
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
       
        //foreach realisation
        int counter;
        
        List<string> linkList;

        public ParallelWorker(List<string> links)
        {
            linkList = links;
        }

        public async void Run()
        {
            ParallelOptions parallelOptions = new ParallelOptions();

            var Proccessor = Environment.ProcessorCount;

            parallelOptions.MaxDegreeOfParallelism = Proccessor;

            int last = 10;

             Parallel.For(0, last, parallelOptions, async (i, loop) => {

                var ChildWikiParcer = new BaseParser(linkList[i]);
                var isConnected = await ChildWikiParcer.Connect();

                 if (isConnected)
                 {
                     ChildWikiParcer.ParceAsync();
                     Console.WriteLine($"Link = " + linkList[i] + " has " + ChildWikiParcer.GetWordCount() + " words.");
                 }

                 // if link doesn't work, we will add one link
                 else if (linkList.Count + 1 > last)
                     last++;
             });
            /*
            //
            Parallel.ForEach(linkList, parallelOptions, (link,loop) => { 

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
            */
        }
    }
}
