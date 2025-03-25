using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOpcUaTiaPortal.Events
{

    public static class EventsInCreateConfig
    {//findItem

        /// <summary>
        /// Remove item into DB Block
        /// </summary>
        public static void RemoveItemFromDbBlock(List<itemDB> itemsDB, List<string> names)//usuwanie rzeczy we wnątrz struktury
        {

            foreach (itemDB itemDB in itemsDB)
            {
                if (names.Count == 1 && names[0] == itemDB.Name)
                {
                    itemsDB.Remove(itemDB);
                    break;
                }
                else if (names[names.Count - 1] == itemDB.Name)
                {
                    RemoveItemFromDbBlock(itemDB._item, names.GetRange(0, names.Count - 1));
                    break;
                }

            }
        }




    }
}
