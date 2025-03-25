using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOpcUaTiaPortal.Events
{
    public static class CreateNameToDatabasePath
    {

            public static void Create(string name, List<itemDB> itemsDB, StringBuilder config)
            {
                foreach (itemDB itemDB in itemsDB)
                {


                    if (itemDB._item.Count != 0)
                    {


                    Create(name + "\"" + itemDB.Name + "\".", itemDB._item, config);
                    }
                    else
                    {
                        StringBuilder send = new StringBuilder();
                        send
                       .Append(name + "\"" + itemDB.Name + "\"");
                        itemDB.Path = send.ToString();
                    }
                }


            }
        
    }
}
