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

        public List<string> GetChaildLinks()
        {
            return links;
        }

        public int GetWordCount()
        {
            return wordsCount;
        }

        public async void ParceAsync()
        {
            var url = Link;
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            var doc = web.Load(url);

            links.Clear();


                // Get the value of the HREF attribute
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    links.Add(hrefValue);
                }
                //
                var getBody = doc.DocumentNode.SelectSingleNode("//body");

            var bigRegexPattern = Regex.Replace(getBody.InnerHtml, @"(<bdi.*?>((.|\n)*?)<\/bdi>)|(<script.*?>((.|\n)*?)<\/script>)|(<style.*?>((.|\n)*?)<\/style>)", string.Empty).Trim();

       /*     var htmlwoutBdi = Regex.Replace(getBody.InnerHtml, @"<bdi.*?>((.|\n)*?)<\/bdi>", string.Empty).Trim();
            var htmlwoutScript = Regex.Replace(htmlwoutBdi, @"<script.*?>((.|\n)*?)<\/script>", string.Empty).Trim(); 
            var htmlwoutStyle = Regex.Replace(htmlwoutScript, @"<style.*?>((.|\n)*?)<\/style>", string.Empty).Trim(); ;*/

            var doc1 = new HtmlDocument();
            doc1.LoadHtml(bigRegexPattern);
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
