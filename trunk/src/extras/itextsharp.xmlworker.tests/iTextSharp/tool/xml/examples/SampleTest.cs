using System;
using System.IO;
using System.Text;
using iTextSharp.testutils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using NUnit.Framework;

namespace itextsharp.xmlworker.tests.iTextSharp.tool.xml.examples {
    public class SampleTest : ITextTest {
        protected String inputPath;
        protected static String testPath;
        protected static String testName;
        protected String outPath;

        protected String inputHtml;

        private const String differenceImagePrefix = "difference";

        protected static string RESOURCES = @"..\..\resources\com\itextpdf\";

        [SetUp]
        public virtual void SetUp() {
            testPath = this.GetType().Namespace.Replace("itextsharp.xmlworker.tests.iTextSharp.", "");
            testPath = testPath.Replace(".", Path.DirectorySeparatorChar.ToString());
            testName = GetTestName();

            outPath = String.Format("target/{0}/{1}/", testPath, GetTestName());
            inputPath = String.Format("{0}/{1}/{2}/", RESOURCES, testPath, GetTestName());
            inputHtml = String.Format("{0}{1}.html", inputPath, GetTestName());

            if (Directory.Exists(outPath))
                DeleteFiles(outPath);
            else
                Directory.CreateDirectory(outPath);
        }

        [Test, Timeout(120000)]
        public virtual void Test() {
            if (this.GetType() != typeof (SampleTest) && (GetTestName().Length > 0)) {
                SetUp();
                base.RunTest();
            }
        }

        protected override void MakePdf(String outPdf) {
            Document doc = new Document(PageSize.A4);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(outPdf, FileMode.Create));
            doc.Open();
            FileStream cssFileStream = new FileStream(RESOURCES + @"tool\xml\examples\sampleTest.css", FileMode.Open, FileAccess.Read, FileShare.Read);
            TransformHtml2Pdf(doc, pdfWriter, new SampleTestImageProvider(),
                new XMLWorkerFontProvider(RESOURCES + @"tool\xml\examples\fonts\"), cssFileStream);
            cssFileStream.Close();
            doc.Close();
        }

        protected override String GetOutPdf() {
            return String.Format("{0}{1}.pdf", outPath, GetTestName());
        }

        protected override String GetCmpPdf() {
            return String.Format("{0}{1}.pdf", inputPath, GetTestName());
        }

        protected override void ComparePdf(String outPdf, String cmpPdf) {
            if (!DetectCrashesAndHangUpsOnly()) {
                CompareTool compareTool = new CompareTool(outPdf, cmpPdf);
                String errorMessage = null;
                if (CompareByContent())
                    errorMessage = compareTool.CompareByContent(outPath, differenceImagePrefix);
                else
                    errorMessage = compareTool.Compare(outPath, differenceImagePrefix);
                if (errorMessage != null) {
                    Assert.Fail(errorMessage);
                }
            }
        }

        protected virtual String GetTestName() {
            return "";
        }

        protected virtual bool DetectCrashesAndHangUpsOnly() {
            return false;
        }

        protected virtual bool CompareByContent() {
            return true;
        }

        protected class SampleTestImageProvider : AbstractImageProvider {
            private String imageRootPath;

            public SampleTestImageProvider() {
                imageRootPath = String.Format("{0}/{1}/{2}/", RESOURCES, testPath, testName);
            }

            public override String GetImageRootPath() {
                return imageRootPath;
            }
        }

        protected virtual void TransformHtml2Pdf(Document doc, PdfWriter pdfWriter, IImageProvider imageProvider,
            IFontProvider fontProvider, Stream cssFile) {
            CssFilesImpl cssFiles = new CssFilesImpl();
            if (cssFile == null)
                cssFile =
                    typeof (XMLWorker).Assembly.GetManifestResourceStream("iTextSharp.tool.xml.css.default.css");
            cssFiles.Add(XMLWorkerHelper.GetCSS(cssFile));
            StyleAttrCSSResolver cssResolver = new StyleAttrCSSResolver(cssFiles);
            HtmlPipelineContext hpc;

            if (fontProvider != null)
                hpc = new HtmlPipelineContext(new CssAppliersImpl(fontProvider));
            else
                hpc = new HtmlPipelineContext(null);

            hpc.SetImageProvider(imageProvider);
            hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(Tags.GetHtmlTagProcessorFactory());
            HtmlPipeline htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, pdfWriter));
            IPipeline pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
            XMLWorker worker = new XMLWorker(pipeline, true);
            XMLParser xmlParse = new XMLParser(true, worker, Encoding.GetEncoding("UTF-8"));
            xmlParse.Parse(File.OpenRead(inputHtml), Encoding.GetEncoding("UTF-8"));
        }
    }
}
