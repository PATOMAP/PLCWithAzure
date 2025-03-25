using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientOpcUaTiaPortal.ListbBoxMainApplication;
using ClientOpcUaTiaPortal.item;
using System.Windows.Controls;
namespace ClientOpcUaTiaPortal.Events
{
    public static class AddToListBox
    {
        /// <summary>
        /// add items to list
        /// </summary>
        /// <param name="items"></param>
        /// <param name="eventWithServer"></param>
        /// <returns></returns>
        public static List<object> AddL(List<itemDB> items,EventWithServer eventWithServer)
        {
            List<object> list = new List<object>();
            foreach (itemDB itemDB in items)
            {
                if (itemDB._item.Count == 0)
                {
                    list.Add(GridClass.CreateDynamicGrid(itemDB, eventWithServer));
                }
                else
                {
                    Expander exToAdd = new Expander();
                    exToAdd.Header = GridClass.AddExpanderToStructOrArray(itemDB.Name);
                    List<object> temp = AddL(itemDB._item,eventWithServer);
                    ListBox boxTemp = new ListBox();
                    foreach (var item in temp)
                    {
                        boxTemp.Items.Add(item);
                    }
                    exToAdd.Content = boxTemp;
                    list.Add(exToAdd);

                }
            }
            return list;

        }
    }
}
