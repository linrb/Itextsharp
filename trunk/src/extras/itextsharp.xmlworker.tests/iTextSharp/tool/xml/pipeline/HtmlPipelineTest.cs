using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.ctx;
using iTextSharp.tool.xml.pipeline.html;
using NUnit.Framework;

namespace itextsharp.xmlworker.tests.iTextSharp.tool.xml.pipeline {
    internal class HtmlPipelineTest {
        private HtmlPipeline p;
        private WorkerContextImpl wc;
        private HtmlPipelineContext hpc;
        private static String b;
        /**
	 * @
	 *
	 */

        [SetUp]
        public void SetUp() {
            hpc = new HtmlPipelineContext(null);
            p = new HtmlPipeline(hpc, null);
            wc = new WorkerContextImpl();
            p.Init(wc);
        }

        [TearDown]
        public void TearDown() {
            b = null;
        }

        private class CustomTagProcessor : ITagProcessor {
            public IList<IElement> StartElement(IWorkerContext ctx,
                Tag tag) {
                return new List<IElement>(0);
            }

            public
                bool IsStackOwner
                () {
                return false;
            }

            public
                IList<IElement> EndElement
                (IWorkerContext ctx, Tag tag, IList<IElement> currentContent) {
                return new List<IElement>(0);
            }

            public
                IList<IElement> Content
                (IWorkerContext ctx, Tag tag, String content) {
                Assert.AreEqual(b, content);
                return new List<IElement>(0);
            }
        }

        private class CustomTagProcessorFactory : ITagProcessorFactory {
            public void RemoveProcessor(String tag) {
            }

            public
                ITagProcessor GetProcessor
                (String tag, String nameSpace) {
                if (tag.Equals("tag", StringComparison.OrdinalIgnoreCase)) ;
                return new CustomTagProcessor();
            }

            public
                void AddProcessor
                (ITagProcessor processor, params String[]
                    tags) {
            }
        }


        [Test]
        public void Init() {
            Assert.NotNull(p.GetLocalContext(wc));
        }

        [Test]
        public void Text() {
            b = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Default.GetBytes("aeéèàçï"));
            ITagProcessorFactory tagFactory = new CustomTagProcessorFactory();


            ((HtmlPipelineContext) p.GetLocalContext(wc)).SetTagFactory(tagFactory)
                .CharSet(Encoding.GetEncoding("ISO-8859-1"));
            p.Content(wc, new Tag("tag"), b, new ProcessObject());
        }
    }
}