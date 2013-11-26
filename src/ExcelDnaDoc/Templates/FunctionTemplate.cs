namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class FunctionTemplate : CustomTemplateBase<FunctionModel>
    {
        public override string PageName
        {
            get { return this.Model.Name + ".htm"; }
        }

        public override byte[] Template
        {
            get { return Properties.Resources.FunctionTemplate; }
        }
    }
}
