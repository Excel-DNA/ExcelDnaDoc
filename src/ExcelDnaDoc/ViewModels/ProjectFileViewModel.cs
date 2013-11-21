namespace ExcelDnaDoc.ViewModels
{
    using ExcelDnaDoc.Models;

    public class ProjectFileViewModel : ViewModelBase<AddInModel>
    {
        public override string PageName
        {
            get { return this.Model.DnaFileName + ".hhp"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.ProjectFileView; }
        }
    }
}
