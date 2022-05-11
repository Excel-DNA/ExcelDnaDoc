namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class TableOfContentsView : ViewBase<AddInModel>
    {
        public override string PageName
        {
            get { return "Table of Contents.hhc"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.TableOfContentsTemplate; }
        }    
    }
}
