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
    class BaseParser : IParser
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
                catch (Exception ex)
                {
                    try
                    {

                        htmlDoc = await web.LoadFromWebAsync("http:" + Link);

                        connect = true;

                    }
                    catch (Exception ex1)
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

        public async void ParceAsync()
        {
            var getBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            var splitBody = Regex.Replace(getBody.InnerHtml, @"(<bdi.*?>((.|\n)*?)<\/bdi>)|
                                                               (<script.*?>((.|\n)*?)<\/script>)|
                                                                (<style.*?>((.|\n)*?)<\/style>)",
                                                                string.Empty, RegexOptions.IgnoreCase).Trim();

       /*     var htmlwoutBdi = Regex.Replace(getBody.InnerHtml, @"<bdi.*?>((.|\n)*?)<\/bdi>", string.Empty).Trim();
            var htmlwoutScript = Regex.Replace(htmlwoutBdi, @"<script.*?>((.|\n)*?)<\/script>", string.Empty).Trim(); 
            var htmlwoutStyle = Regex.Replace(htmlwoutScript, @"<style.*?>((.|\n)*?)<\/style>", string.Empty).Trim(); ;*/

            var doc = new HtmlDocument();

            doc.LoadHtml(splitBody);

            var clearBody =  doc.DocumentNode.InnerText;

            clearBody = Regex.Replace(clearBody, @"(\W+)|\d+", " ");
            var info = clearBody.Where(t => t == ' ').AsParallel();
            wordsCount = info.Count() + 1;

        }
    }
}
