using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOpcUaTiaPortal.CheckClass
{
    public static  class CheckIsStruct//checkIsNotStruct
    {


        public static bool Check(string name, listObjectType listObjectType)
        {
            foreach (var item in listObjectType._list)
            {
                if (item.Name == name)
                {
                    if (item._itemDB != null || name == "Array" || name == "Struct")
                        return false;
                    else
                        return true;
                }


            }
            return true;
        }
    }
}
