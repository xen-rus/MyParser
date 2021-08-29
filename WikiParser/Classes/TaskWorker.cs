using System;
using System.Collections.Generic;
using System.Text;
using WikiParser.Interfaces;
using System.Threading.Tasks;
using WikiParcer.Classes;

namespace WikiParser.Classes
{
    class TaskWorker : IThreadWorker
    {
        List<string> linkList;

        public TaskWorker(List<string> links)
        {
            linkList = links;

        }
        public void Run()
        {
            Task<bool> [] tasks = new Task<bool>[10];

            int maxLink = 10;

            for (int i = 0; i < maxLink; i++)
            {
                string link = linkList[i];

                tasks[i] = Task.Run(() => Parse(link));

                if(i != 0 && i % 2 ==0 )
                    Task.WaitAny(tasks[i], tasks[i - 1]);


                // if link doesn't work, we will add one link
                if (i != 0 && 
                    !(tasks[i].Result && tasks[i - 1].Result) &&
                    (linkList.Count + 1 > maxLink) )
                   maxLink++;
            }
            
           // Task.WaitAll(tasks);
            Console.WriteLine($"Ready");
            
        }

        private async Task<bool>  Parse(string link)
        {
            var ChildWikiParcer = new BaseParser(link);
            
            bool isConnected = await ChildWikiParcer.Connect();

            if (isConnected)
            {
                ChildWikiParcer.ParceAsync();
                Console.WriteLine($"Link =" + link + " has " + ChildWikiParcer.GetWordCount() + " words.");
            }
            return isConnected;

        }

    }
}
