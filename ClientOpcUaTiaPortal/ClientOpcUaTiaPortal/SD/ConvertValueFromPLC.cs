using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOpcUaTiaPortal.SD
{
    public class ConvertValueFromPLC
    {
        public static object ConvertVar(string type, string value)
        {
            object convertedValue = null;  // Variable to hold the converted value

            switch (type)
            {
                case "Int16":
                    try
                    {
                        convertedValue = Convert.ToInt16(value);
                    }
                    catch
                    {

                        convertedValue = null;
                    }
                    break;
                case "String":
                    try
                    {
                        convertedValue = Convert.ToString(value);
                    }
                    catch
                    {

                        convertedValue = null;
                    }
                    break;
                case "Real":
                    try
                    {
                        convertedValue = Convert.ToSingle(value);  // float conversion
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "Bool":
                    try
                    {
                        convertedValue = Convert.ToBoolean(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "Byte":
                    try
                    {
                        convertedValue = Convert.ToByte(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "SByte":
                    try
                    {
                        convertedValue = Convert.ToSByte(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "UInt16":
                    try
                    {
                        convertedValue = Convert.ToUInt16(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "Int32":
                    try
                    {
                        convertedValue = Convert.ToInt32(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "UInt32":
                    try
                    {
                        convertedValue = Convert.ToUInt32(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "Int64":
                    try
                    {
                        convertedValue = Convert.ToInt64(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "UInt64":
                    try
                    {
                        convertedValue = Convert.ToUInt64(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                case "Double":
                    try
                    {
                        convertedValue = Convert.ToDouble(value);
                    }
                    catch
                    {
                        convertedValue = null;
                    }
                    break;
                default:
                    convertedValue = null;
                    break;
            }

            return convertedValue;
        }
    }
}
