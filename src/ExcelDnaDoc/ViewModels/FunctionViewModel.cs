namespace ExcelDnaDoc.ViewModels
{
    using ExcelDnaDoc.Models;

    public class FunctionViewModel : ViewModelBase<FunctionModel>
    {
        public override string PageName
        {
            get { return this.Model.Name + ".htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.FunctionView; }
        }
    }
}
