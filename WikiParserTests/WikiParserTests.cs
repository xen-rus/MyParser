using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiParcer.Classes;
using WikiParser.Classes;

namespace WikiParserTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task WikiConnectionTest()
        {
            const string WikiLink = "http://www.wikipedia.org";

            BaseParser wikiParser = new BaseParser(WikiLink);
            var isConnect = await wikiParser.Connect();

            Assert.IsTrue(isConnect);
        }

        [Test]
        public async Task BadConnectionTest()
        {
            const string WikiLink = "http://www.wiki111pedia.org";

            BaseParser wikiParser = new BaseParser(WikiLink);
            var isConnect = await wikiParser.Connect();

            Assert.IsTrue(!isConnect);
        }


        [Test]
        public async Task ParallelNullArrayTest ()
        {
            const string WikiLink = "http://www.wikipedia.org";

            BaseParser wikiParser = new BaseParser(WikiLink);
            var isConnect = await wikiParser.Connect();

            ParallelWorker parallelWorker = new ParallelWorker(null);

            Assert.DoesNotThrow(() => parallelWorker.Run());
        }

        [Test]
        public async Task TasksNullArrayTest()
        {
            TaskWorker parallelWorker = new TaskWorker(null);
            //Assert.

            Assert.DoesNotThrow(() => parallelWorker.Run());
        }


        [Test]
        public async Task metanitParceTest()
        {
            const string Link = "//metanit.com";

            BaseParser wikiParser = new BaseParser(Link);
            var isConnect = await wikiParser.Connect();
            wikiParser.Parse();
            var info = wikiParser.GetWordCount();
           // List<string> list = new List<string>();

            //list.Add(WikiLink);
            Assert.IsTrue(info == 253); 
        }
      
    }
}