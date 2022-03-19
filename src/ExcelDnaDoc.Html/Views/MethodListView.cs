namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class MethodListView : ViewBase<AddInModel>
    {
        public override string PageName
        {
            get { return "index.htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.MethodListTemplate; }
        }
    }
}
