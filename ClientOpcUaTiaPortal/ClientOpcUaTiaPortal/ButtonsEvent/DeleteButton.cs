using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ClientOpcUaTiaPortal.Events;

namespace ClientOpcUaTiaPortal.ButtonsEvent
{
    public static class DeleteButton
    {
        public static void AddDeleteEvent(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            List<string> names = new List<string>();
            Grid grid = deleteButton.Parent as Grid;
            Expander ex = null;
            ListBox listbox;


            if (grid.Parent.GetType() == typeof(ListBox))
            {
                listbox = grid.Parent as ListBox;
                UIElementCollection items = grid.Children as UIElementCollection;
                names.Add((items[0] as TextBox).Text.ToString());
                ListBox temp = listbox;
                while (temp.Parent.GetType() == typeof(Expander))
                {
                    Expander exinStruct = temp.Parent as Expander;
                    Grid gridExp = exinStruct.Header as Grid;
                    UIElementCollection itemsEx = gridExp.Children as UIElementCollection;
                    names.Add((itemsEx[0] as TextBlock).Text.ToString());
                    temp = exinStruct.Parent as ListBox;

                }
            }
            else
            {
                ex = (Expander)grid.Parent;
                listbox = ex.Parent as ListBox;
                UIElementCollection items = grid.Children as UIElementCollection;
                names.Add((items[0] as TextBlock).Text.ToString());
                ListBox temp = listbox;
                while (temp.Parent.GetType() == typeof(Expander))
                {
                    Expander exinStruct = temp.Parent as Expander;
                    Grid gridExp = exinStruct.Header as Grid;
                    UIElementCollection itemsEx = gridExp.Children as UIElementCollection;
                    names.Add((itemsEx[0] as TextBlock).Text.ToString());
                    temp = exinStruct.Parent as ListBox;

                }

            }

            if (deleteButton.DataContext.GetType() == typeof(dbBlock))
            {
                dbBlock dbDeleteItem = deleteButton.DataContext as dbBlock;
                EventsInCreateConfig.RemoveItemFromDbBlock(dbDeleteItem._itemsDB, names);
            }
            else if (deleteButton.DataContext.GetType() == typeof(itemDB))
            {
                itemDB dbDeleteItem = deleteButton.DataContext as itemDB;
                EventsInCreateConfig.RemoveItemFromDbBlock(dbDeleteItem._item, names);

            }
            if (ex == null)
                listbox.Items.Remove(grid);
            else
                listbox.Items.Remove(ex);

        }
    }
}
