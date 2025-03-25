using ClientOpcUaTiaPortal.ButtonsEvent;
using ClientOpcUaTiaPortal.Events;
using ClientOpcUaTiaPortal.item;
using InfluxDB.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClientOpcUaTiaPortal.ListbBoxMainApplication
{
    public static class GridClass
    {
        public static Grid AddExpanderToStructOrArray(string name)
        {
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            Button modifyButton = new Button { Content = "Delete", Width = 60, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(5) };
            modifyButton.Click += DeleteButton.AddDeleteEvent;
            TextBlock Txtblock = new TextBlock { Text = name, Margin = new Thickness(5) };

            Grid.SetRow(Txtblock, 0);
            Grid.SetColumn(Txtblock, 0);

            Grid.SetRow(modifyButton, 0);
            Grid.SetColumn(modifyButton, 1);

            grid.Children.Add(Txtblock);
            grid.Children.Add(modifyButton);

            return grid;
        }
        public static Grid CreateDynamicGrid(itemDB item, EventWithServer eventWithServer)
        {

            Grid grid = new Grid { HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(2, 0, 2, 0) };
            var binding = new Binding("BackgroundColor")
            {
                Source = item
            };
            grid.SetBinding(Grid.BackgroundProperty, binding);
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });


            TextBox nameTextBox = new TextBox { Text = item.Name };
            Grid.SetColumn(nameTextBox, 0);
            grid.Children.Add(nameTextBox);


            TextBox typeTextBox = new TextBox { Text = item.type1 };
            Grid.SetColumn(typeTextBox, 1);
            grid.Children.Add(typeTextBox);


            //TextBox valueTextBox = new TextBox { Text = item.Value };
            TextBox valueTextBox = new TextBox();

            var bindingValue = new Binding("Value")
            { Source = item };

            valueTextBox.SetBinding(TextBox.TextProperty, bindingValue);
            valueTextBox.KeyDown += eventWithServer.ChangeTextInValue;
            Grid.SetColumn(valueTextBox, 2);
            grid.Children.Add(valueTextBox);

            if (item.arrayAct == null || item.arrayAct == false)
            {
                Button modifyButton = new Button { Content = "Delete", Width = 60, HorizontalAlignment = HorizontalAlignment.Center };
                modifyButton.Click += DeleteButton.AddDeleteEvent;
                Grid.SetColumn(modifyButton, 3);
                grid.Children.Add(modifyButton);
            }

            return grid;
        }

    }
}
