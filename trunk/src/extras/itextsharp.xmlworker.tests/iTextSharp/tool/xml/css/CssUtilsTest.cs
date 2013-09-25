using System;
using System.Collections.Generic;
using iTextSharp.text.log;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using NUnit.Framework;

namespace itextsharp.xmlworker.tests.iTextSharp.tool.xml.css {
    /**
 * @author redlab_b
 *
 */

    internal class CssUtilsTest {
        private static int MAX = 10000;
        private CssUtils css;
        private String str;

        [SetUp]
        public void SetUp() {
            LoggerFactory.GetInstance().SetLogger(new SysoLogger(3));
            css = CssUtils.GetInstance();
            str = "  een  twee   drie    vier    een  twee   drie    vier";
        }

        [Test]
        public void CalculateHorizontalMargin() {
            Tag t = new Tag(str);
            t.CSS["margin-left"] = "15pt";
            t.CSS["margin-right"] = "15pt";
            Assert.AreEqual(30, css.GetLeftAndRightMargin(t, 0f), 0);
        }

        [Test]
        public void ValidateMetricValue() {
            Assert.AreEqual(true, css.IsMetricValue("px"));
            Assert.AreEqual(true, css.IsMetricValue("in"));
            Assert.AreEqual(true, css.IsMetricValue("cm"));
            Assert.AreEqual(true, css.IsMetricValue("mm"));
            Assert.AreEqual(true, css.IsMetricValue("pc"));
            Assert.AreEqual(false, css.IsMetricValue("em"));
            Assert.AreEqual(false, css.IsMetricValue("ex"));
            Assert.AreEqual(true, css.IsMetricValue("pt"));
            Assert.AreEqual(true, css.IsMetricValue("inch"));
            Assert.AreEqual(false, css.IsMetricValue("m"));
        }

        [Test]
        public void ValidateNumericValue() {
            Assert.AreEqual(true, css.IsNumericValue("1"));
            Assert.AreEqual(true, css.IsNumericValue("12"));
            Assert.AreEqual(true, css.IsNumericValue("1.2"));
            Assert.AreEqual(true, css.IsNumericValue(".12"));
            Assert.AreEqual(false, css.IsNumericValue("12f"));
            Assert.AreEqual(false, css.IsNumericValue("f1.2"));
            Assert.AreEqual(false, css.IsNumericValue(".12f"));
        }

        [Test]
        public void ParseLength() {
            Assert.AreEqual(9, css.ParsePxInCmMmPcToPt("12"), 0);
            Assert.AreEqual(576, css.ParsePxInCmMmPcToPt("8inch"), 0);
            Assert.AreEqual(576, css.ParsePxInCmMmPcToPt("8", CSS.Value.IN), 0);
        }

        [Test]
        public void SplitFont() {
            IDictionary<String, String> processFont = css.ProcessFont("bold italic 16pt/3px Verdana");
            Assert.AreEqual("bold", processFont["font-weight"]);
            Assert.AreEqual("italic", processFont["font-style"]);
            Assert.AreEqual("16pt", processFont["font-size"]);
            Assert.AreEqual("3px", processFont["line-height"]);
            Assert.AreEqual("Verdana", processFont["font-family"]);
        }

        [Test]
        public void SplitBackgroundOne() {
            IDictionary<String, String> background =
                css.ProcessBackground("#00ff00 url('smiley.gif') no-repeat fixed center top");
            Assert.AreEqual("#00ff00", background["background-color"]);
            Assert.AreEqual("url('smiley.gif')", background["background-image"]);
            Assert.AreEqual("no-repeat", background["background-repeat"]);
            Assert.AreEqual("fixed", background["background-attachment"]);
            Assert.AreEqual("top center", background["background-position"]);
        }

        [Test]
        public void SplitBackgroundTwo() {
            IDictionary<String, String> background = css
                .ProcessBackground("rgdbq(150, 90, 60) url'smiley.gif') repeat-x scroll 20 60%");

            // the original JAVA assert: Assert.assertEquals(null, background.get("background-color"));
            // we assume that background["background-color"] value couldn't have been set to null.
            Assert.IsFalse(background.ContainsKey("background-color"));
            Assert.IsFalse(background.ContainsKey("background-image"));

            Assert.AreEqual("repeat-x", background["background-repeat"]);
            Assert.AreEqual("scroll", background["background-attachment"]);
            Assert.AreEqual("60% 20", background["background-position"]);
        }

        [Test]
        public void SplitBackgroundThree() {
            IDictionary<String, String> background = css.ProcessBackground("DarkOliveGreen fixed center");
            Assert.AreEqual("DarkOliveGreen", background["background-color"]);


            Assert.IsFalse(background.ContainsKey("background-image"));
            Assert.IsFalse(background.ContainsKey("background-repeat"));

            Assert.AreEqual("fixed", background["background-attachment"]);
            Assert.AreEqual("center", background["background-position"]);
        }

        [Test]
        public void ReplaceDoubleSpaces() {
            String stripDoubleSpacesAndTrim = css.StripDoubleSpacesAndTrim(str);
            Assert.IsTrue(!(stripDoubleSpacesAndTrim.Contains("  ")), "double spaces [  ] detected");
        }

        [Test]
        public void Parse1BoxValuesTest() {
            String box = "2px";
            IDictionary<String, String> values = css.ParseBoxValues(box, "pre-", "-post");
            ValidateKeys(values);
            Assert.AreEqual(box, values["pre-right-post"]);
            Assert.AreEqual(box, values["pre-left-post"]);
            Assert.AreEqual(box, values["pre-bottom-post"]);
            Assert.AreEqual(box, values["pre-top-post"]);
        }

        /**
	 * @param values
	 */

        private static void ValidateKeys(IDictionary<String, String> values) {
            Assert.IsTrue(values.ContainsKey("pre-top-post"), "key not found top");
            Assert.IsTrue(values.ContainsKey("pre-bottom-post"), "key not found bottom");
            Assert.IsTrue(values.ContainsKey("pre-left-post"), "key not found left");
            Assert.IsTrue(values.ContainsKey("pre-right-post"), "key not found right");
        }

        [Test]
        public void Parse2BoxValuesTest() {
            String box = "2px 5px";
            IDictionary<String, String> values = css.ParseBoxValues(box, "pre-", "-post");
            ValidateKeys(values);
            Assert.AreEqual("5px", values["pre-right-post"]);
            Assert.AreEqual("5px", values["pre-left-post"]);
            Assert.AreEqual("2px", values["pre-bottom-post"]);
            Assert.AreEqual("2px", values["pre-top-post"]);
        }

        [Test]
        public void Parse3BoxValuesTest() {
            String box = "2px 3px 4px";
            IDictionary<String, String> values = css.ParseBoxValues(box, "pre-", "-post");
            ValidateKeys(values);
            Assert.AreEqual("3px", values["pre-right-post"]);
            Assert.AreEqual("3px", values["pre-left-post"]);
            Assert.AreEqual("4px", values["pre-bottom-post"]);
            Assert.AreEqual("2px", values["pre-top-post"]);
        }

        [Test]
        public void Parse4BoxValuesTest() {
            String box = "2px 3px 4px 5px";
            IDictionary<String, String> values = css.ParseBoxValues(box, "pre-", "-post");
            ValidateKeys(values);
            Assert.AreEqual("3px", values["pre-right-post"]);
            Assert.AreEqual("5px", values["pre-left-post"]);
            Assert.AreEqual("4px", values["pre-bottom-post"]);
            Assert.AreEqual("2px", values["pre-top-post"]);
        }

        [Test]
        public void ParseBorder() {
            String border = "dashed";
            IDictionary<String, String> map = css.ParseBorder(border);
            Assert.IsTrue(map.ContainsKey("border-left-style"));
            Assert.AreEqual("dashed", map["border-left-style"]);
            Assert.IsTrue(map.ContainsKey("border-top-style"));
            Assert.AreEqual("dashed", map["border-top-style"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-style"));
            Assert.AreEqual("dashed", map["border-bottom-style"]);
            Assert.IsTrue(map.ContainsKey("border-right-style"));
            Assert.AreEqual("dashed", map["border-right-style"]);
        }

        [Test]
        public void ParseBorder2() {
            String border = "dashed green";
            IDictionary<String, String> map = css.ParseBorder(border);
            Assert.IsTrue(map.ContainsKey("border-left-style"));
            Assert.AreEqual("dashed", map["border-left-style"]);
            Assert.IsTrue(map.ContainsKey("border-top-style"));
            Assert.AreEqual("dashed", map["border-top-style"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-style"));
            Assert.AreEqual("dashed", map["border-bottom-style"]);
            Assert.IsTrue(map.ContainsKey("border-right-style"));
            Assert.AreEqual("dashed", map["border-right-style"]);
            Assert.IsTrue(map.ContainsKey("border-left-color"));
            Assert.AreEqual("green", map["border-left-color"]);
            Assert.IsTrue(map.ContainsKey("border-top-color"));
            Assert.AreEqual("green", map["border-top-color"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-color"));
            Assert.AreEqual("green", map["border-bottom-color"]);
            Assert.IsTrue(map.ContainsKey("border-right-color"));
            Assert.AreEqual("green", map["border-right-color"]);
        }

        [Test]
        public void ParseBorder3() {
            String border = "1px dashed";
            IDictionary<String, String> map = css.ParseBorder(border);
            Assert.IsTrue(map.ContainsKey("border-left-style"));
            Assert.AreEqual("dashed", map["border-left-style"]);
            Assert.IsTrue(map.ContainsKey("border-top-style"));
            Assert.AreEqual("dashed", map["border-top-style"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-style"));
            Assert.AreEqual("dashed", map["border-bottom-style"]);
            Assert.IsTrue(map.ContainsKey("border-right-style"));
            Assert.AreEqual("dashed", map["border-right-style"]);
            Assert.IsTrue(map.ContainsKey("border-left-width"));
            Assert.AreEqual("1px", map["border-left-width"]);
            Assert.IsTrue(map.ContainsKey("border-top-width"));
            Assert.AreEqual("1px", map["border-top-width"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-width"));
            Assert.AreEqual("1px", map["border-bottom-width"]);
            Assert.IsTrue(map.ContainsKey("border-right-width"));
            Assert.AreEqual("1px", map["border-right-width"]);
        }

        [Test]
        public void ParseBorder4() {
            String border = "1px dashed green";
            IDictionary<String, String> map = css.ParseBorder(border);
            Assert.IsTrue(map.ContainsKey("border-left-style"));
            Assert.AreEqual("dashed", map["border-left-style"]);
            Assert.IsTrue(map.ContainsKey("border-top-style"));
            Assert.AreEqual("dashed", map["border-top-style"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-style"));
            Assert.AreEqual("dashed", map["border-bottom-style"]);
            Assert.IsTrue(map.ContainsKey("border-right-style"));
            Assert.AreEqual("dashed", map["border-right-style"]);
            Assert.IsTrue(map.ContainsKey("border-left-color"));
            Assert.AreEqual("green", map["border-left-color"]);
            Assert.IsTrue(map.ContainsKey("border-top-color"));
            Assert.AreEqual("green", map["border-top-color"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-color"));
            Assert.AreEqual("green", map["border-bottom-color"]);
            Assert.IsTrue(map.ContainsKey("border-right-color"));
            Assert.AreEqual("green", map["border-right-color"]);
            Assert.IsTrue(map.ContainsKey("border-left-width"));
            Assert.AreEqual("1px", map["border-left-width"]);
            Assert.IsTrue(map.ContainsKey("border-top-width"));
            Assert.AreEqual("1px", map["border-top-width"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-width"));
            Assert.AreEqual("1px", map["border-bottom-width"]);
            Assert.IsTrue(map.ContainsKey("border-right-width"));
            Assert.AreEqual("1px", map["border-right-width"]);
            Assert.IsTrue(map.ContainsKey("border-left-width"));
            Assert.AreEqual("1px", map["border-left-width"]);
            Assert.IsTrue(map.ContainsKey("border-top-width"));
            Assert.AreEqual("1px", map["border-top-width"]);
            Assert.IsTrue(map.ContainsKey("border-bottom-width"));
            Assert.AreEqual("1px", map["border-bottom-width"]);
            Assert.IsTrue(map.ContainsKey("border-right-width"));
            Assert.AreEqual("1px", map["border-right-width"]);
        }

        [Test]
        public void ParseUrlSingleQuoted() {
            Assert.AreEqual("file.jpg", css.ExtractUrl("url( 'file.jpg')"));
        }

        [Test]
        public void ParseUrlDoubleQuoted() {
            Assert.AreEqual("file.jpg", css.ExtractUrl("url ( \"file.jpg\" )"));
        }

        [Test]
        public void ParseUnparsableUrl() {
            Assert.AreEqual("('file.jpg')", css.ExtractUrl("('file.jpg')"));
        }
    }
}