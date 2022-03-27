using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ExcelDna.Integration;

namespace ExcelDnaDoc.Utility
{
    public static class ExcelDnaHelper
    {
        public static bool IsValidCommand(MethodInfo method)
        {
            if (!(method.IsPublic && method.IsStatic))
            {
                return false;
            }

            return Attribute.IsDefined(method, typeof(ExcelCommandAttribute));
        }
    }
}
