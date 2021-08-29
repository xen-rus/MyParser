using System;
using System.Collections.Generic;
using System.Text;
using WikiParcer.Interfaces;

using HtmlAgilityPack;
using System.Text.RegularExpressions;

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WikiParcer.Classes
{
    public class BaseParser : IParser
    {
        string Link { get; set; }

        private int wordsCount;

        List<string> links = new List<string>();

        private HtmlDocument htmlDoc;

        public  List<string> GetChaildLinks()
        {
            links?.Clear();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                links.Add(hrefValue);
            }

            return links;
        }

        public BaseParser(string mainLink)
        {
            Link = mainLink;
        }


        //We can create loop to connect ;
        public async Task<bool> Connect() 
        {
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            bool connect = false; ;
            int counter = 0;
            while (!connect)
            {
                if (counter == 5)
                    break;

                try
                {
                    if (Link.Contains("http:") ||
                       Link.Contains("https:"))
                        htmlDoc = web.Load(Link, "GET");
                    else
                        htmlDoc = web.Load("https:" + Link, "GET");

                    connect = true;
                }
                catch 
                {
                    try
                    {
                        htmlDoc = await web.LoadFromWebAsync("http:" + Link);
                        connect = true;
                    }
                    catch 
                    {
                        counter++;
                        connect = false;
                    }
                }
            }


            if(!connect)
                Console.WriteLine($"Can't connect to " + Link);

            return connect;
        }

        public int GetWordCount()
        {
            return wordsCount;
        }

        public void Parce()
        {
            var getBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            var splitBody = Regex.Replace(getBody.InnerHtml, @"(.<bdi.*?>((.|\n)*?)<\/bdi>)",
                                                                " ", RegexOptions.IgnoreCase ).Trim();


            List<Regex> RegexArray = new List<Regex>() {
                                                        new Regex(@"(<script.*?>((.|\n)*?)<\/script>)"),
                                                        new Regex(@"(<noscript.*?>((.|\n)*?)<\/noscript>)"),
                                                        new Regex (@"(<style.*?>((.|\n)*?)<\/style>)") 
                                                        };

            for(int i = 0; i < RegexArray.Count; i++)
            {
                splitBody = RegexArray[i].Replace(splitBody,String.Empty).Trim();
            }

            var doc = new HtmlDocument();

            doc.LoadHtml(splitBody);

            var clearBody =  doc.DocumentNode.InnerText;

            clearBody = Regex.Replace(clearBody, @"(\W+)|\d+", " ").Trim();

            if ( (clearBody.IndexOf("nbsp") != -1) || (clearBody.IndexOf("raquo") != -1))
            {
                clearBody = Regex.Replace(clearBody, @"nbsp|raquo|quot", "").Trim();
            }
            clearBody = Regex.Replace(clearBody, @"\s+", " ", RegexOptions.Multiline);

            //           var test1 = clearBody.Split();    //Testing 
            //           var info1 = test1.Except(test);

            var info = clearBody.Where(t => t == ' ').AsParallel();
            wordsCount = info.Count() + 1;

        }
    }
}
