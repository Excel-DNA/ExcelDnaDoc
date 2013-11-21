namespace ExcelDnaDoc.ViewModels
{
    using ExcelDnaDoc.Models;

    public class FunctionGroupViewModel : ViewModelBase<FunctionGroupModel>
    {
        public override string PageName
        {
            get { return this.Model.Name + " Functions.htm"; }
        }

        public override byte[] Template 
        { 
            get { return Properties.Resources.FunctionGroupView; } 
        }
    }
}
