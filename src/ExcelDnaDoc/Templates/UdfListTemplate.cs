namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class UdfListTemplate : CustomTemplateBase<AddInModel>
    {
        public override string PageName
        {
            get { return "index.htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.UdfListTemplate; }
        }
    }
}
