namespace ExcelDnaDoc.ViewModels
{
    using ExcelDnaDoc.Models;

    public class TableOfContentsViewModel : ViewModelBase<AddInModel>
    {
        public override string PageName
        {
            get { return "Table of Contents.hhc"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.TableOfContentsView; }
        }    
    }
}
