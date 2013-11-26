namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class ProjectFileTemplate : CustomTemplateBase<AddInModel>
    {
        public override string PageName
        {
            get { return this.Model.DnaFileName + ".hhp"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.ProjectFileTemplate; }
        }
    }
}
