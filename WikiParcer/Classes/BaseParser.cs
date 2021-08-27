using System;
using System.Collections.Generic;
using System.Text;
using WikiParcer.Interfaces;

using HtmlAgilityPack;
using System.Text.RegularExpressions;

using System.Linq;

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
            // Get the value of the HREF attribute
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue =link.GetAttributeValue("href", string.Empty);

                if(hrefValue.Contains("http:") || 
                   hrefValue.Contains("https:"))
                    links.Add(hrefValue);
                else
                    links.Add("https:" + hrefValue);

            }

            return links;
        }

        public BaseParser(string mainLink)
        {
            Link = mainLink;
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            htmlDoc  = web.Load(mainLink);
        }




        public int GetWordCount()
        {
            return wordsCount;
        }

        public async void ParceAsync()
        {
            var getBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            var splitBody = Regex.Replace(getBody.InnerHtml, @"(<bdi.*?>((.|\n)*?)<\/bdi>)|(<script.*?>((.|\n)*?)<\/script>)|(<style.*?>((.|\n)*?)<\/style>)|(([0-9]|\n)*?)", string.Empty, RegexOptions.IgnoreCase).Trim();

       /*     var htmlwoutBdi = Regex.Replace(getBody.InnerHtml, @"<bdi.*?>((.|\n)*?)<\/bdi>", string.Empty).Trim();
            var htmlwoutScript = Regex.Replace(htmlwoutBdi, @"<script.*?>((.|\n)*?)<\/script>", string.Empty).Trim(); 
            var htmlwoutStyle = Regex.Replace(htmlwoutScript, @"<style.*?>((.|\n)*?)<\/style>", string.Empty).Trim(); ;*/

            var doc1 = new HtmlDocument();
            doc1.LoadHtml(splitBody);
            var cleanBody =  doc1.DocumentNode.InnerText;

            cleanBody = cleanBody.Replace('\n', ' ');
            cleanBody = cleanBody.Replace('/', ' ');

            var strings = cleanBody.Split().Where(line => line != "" && (line.IndexOf("&") == -1)).AsParallel();

            wordsCount = strings.Count();
        }

        public void Route(string link)
        {
            Link = link;
        }
    }
}
