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

        public List<string> GetChaildLinks()
        {
            throw new NotImplementedException();
        }

        public int GetWordCount()
        {
            throw new NotImplementedException();
        }

        public async void ParceAsync()
        {
            var url = Link;
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            var doc = web.Load(url);

           //
            var getBody = doc.DocumentNode.SelectSingleNode("//body");
            var htmlwoutBdi = Regex.Replace(getBody.InnerHtml, @"<bdi.*?>(.*?)<\/bdi>", string.Empty).Trim();
            var htmlwoutScript = Regex.Replace(htmlwoutBdi, @"<script.*?>(.*?)<\/script>", string.Empty).Trim(); ;

            var doc1 = new HtmlDocument();
            doc1.LoadHtml(htmlwoutScript);
            var cleanBody =  doc1.DocumentNode.InnerText;

            cleanBody = cleanBody.Replace('\n', ' ');
            cleanBody = cleanBody.Replace('/', ' ');

            var strings = cleanBody.Split().Where(line => line != "" && (line.IndexOf("&") == -1)).AsParallel();

        }

        public void Route(string link)
        {
            Link = link;
        }
    }
}
