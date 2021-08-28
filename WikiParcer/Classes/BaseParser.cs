using System;
using System.Collections.Generic;
using System.Text;
using WikiParcer.Interfaces;

using HtmlAgilityPack;
using System.Text.RegularExpressions;

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<bool> Connect()
        {
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            try
            {
                if (Link.Contains("http:") ||
                   Link.Contains("https:"))
                    htmlDoc = web.Load(Link, "GET");
                else
                    htmlDoc = web.Load("https:" + Link, "GET");

                return true;
            }
            catch (Exception ex)
            {
                try
                { 

                    htmlDoc = await web.LoadFromWebAsync("http:" + Link);

                    return true;
                
                }
                catch(Exception ex1)
                {
                    Console.WriteLine($"Can't connect to " + Link);

                    return false;
                }
            }


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

            var cleanBody =  doc.DocumentNode.InnerText;

            cleanBody = Regex.Replace(cleanBody, @"(\W+)|\d+", " ");

            var strings = cleanBody.Split();

            wordsCount = strings.Count();
        }

        public void Route(string link)
        {
            Link = link;
        }
    }
}
