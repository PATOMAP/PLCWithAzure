using ClientOpcUaTiaPortal.CheckClass;
using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientOpcUaTiaPortal.SD
{
    /// <summary>
    /// Add type variable to comobox to add change 
    /// </summary>
    public class ComboBoxEvent
    {
        public static void ChangeItemInCombobox(object sender,listObjectType listObjectType)
        {
            ComboBox box = sender as ComboBox;
            Grid grid = box.Parent as Grid;


            UIElementCollection items = grid.Children as UIElementCollection;

            string choiceItem = box.SelectedItem as string;//listType
            string choice = choiceItem;
            TextBlock wartNazwa = items[3] as TextBlock;
            TextBox wart = items[6] as TextBox;

            if (!CheckIsStruct.Check(choice,listObjectType))
            {
                wartNazwa.Visibility = Visibility.Hidden;
                wart.Visibility = Visibility.Hidden;

            }
            else
            {
                wartNazwa.Visibility = Visibility.Visible;
                wart.Visibility = Visibility.Visible;
                wartNazwa.Text = "Value:";
            }
        }
        public static void DropDownInComboBox(object sender,listObjectType listObjectType)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.Items.Count > 0)
                comboBox.Items.Clear();

            foreach (var item in listObjectType._list)
            {
                comboBox.Items.Add(item.Name);
            }



        }


    }

}
