namespace ExcelDnaDoc.ViewModels
{
    using ExcelDnaDoc.Models;

    public class UdfListViewModel : ViewModelBase<AddInModel>
    {
        public override string PageName
        {
            get { return "index.htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.UdfListView; }
        }
    }
}
