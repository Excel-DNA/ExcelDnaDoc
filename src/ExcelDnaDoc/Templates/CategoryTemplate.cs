namespace ExcelDnaDoc.Templates
{
    using ExcelDna.Documentation.Models;

    public class CategoryTemplate : CustomTemplateBase<CategoryModel>
    {
        public override string PageName
        {
            get { return this.Model.Name + " Functions.htm"; }
        }

        public override byte[] Template 
        { 
            get { return Properties.Resources.CategoryTemplate; } 
        }
    }
}
