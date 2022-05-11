namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class CommandListView : ViewBase<AddInModel>
    {
        public override string PageName
        {
            get { return this.Model.ProjectName + " Commands.htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.CommandListTemplate; }
        }
    }
}