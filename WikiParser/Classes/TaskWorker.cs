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
            Task [] tasks = new Task[10];
  
            for (int i = 0; i < 10; i++)
            {
                string link = linkList[i];
                tasks[i] = Task.Run(() =>Parse(link));

            }
            
            Task.WaitAll(tasks);
            Console.WriteLine($"Ready");
            
        }

        public async Task  Parse(string link)
        {
            var ChildWikiParcer = new BaseParser(link);

            await ChildWikiParcer.Connect();

            ChildWikiParcer.ParceAsync();

            Console.WriteLine($"Link =" + link + " has " + ChildWikiParcer.GetWordCount() + " words.");



        }

    }
}
