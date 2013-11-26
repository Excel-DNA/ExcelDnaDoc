namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class FunctionGroupTemplate : CustomTemplateBase<FunctionGroupModel>
    {
        public override string PageName
        {
            get { return this.Model.Name + " Functions.htm"; }
        }

        public override byte[] Template 
        { 
            get { return Properties.Resources.FunctionGroupTemplate; } 
        }
    }
}
