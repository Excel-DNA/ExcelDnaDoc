using ExcelDna.Documentation;
using ExcelDna.Integration;
using System;

namespace Lib
{
    public class ParameterTypes
    {
        [ExcelFunction(Category = "ParameterTypes")]
        public static object P1(string p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P2(DateTime p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P3(Double p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P4(Double[] p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P5(Double[,] p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P6(Object p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P7(Object[] p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P8(Object[,] p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P9(Boolean p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P10(Int32 p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P11(Int16 p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P12(UInt16 p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P13(Decimal p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object P14(Int64 p)
        {
            return null;
        }

        [ExcelFunction(Category = "ParameterTypes")]
        public static object Bad(ParameterTypes p)
        {
            return null;
        }
    }
}
