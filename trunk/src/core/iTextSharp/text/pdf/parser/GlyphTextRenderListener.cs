namespace iTextSharp.text.pdf.parser {
    internal class GlyphTextRenderListener : GlyphRenderListener, ITextExtractionStrategy {
        private ITextExtractionStrategy deleg;

        public GlyphTextRenderListener(ITextExtractionStrategy deleg) : base(deleg) {
            this.deleg = deleg;
        }

        virtual public string GetResultantText() {
            return deleg.GetResultantText();
        }
    }
}
