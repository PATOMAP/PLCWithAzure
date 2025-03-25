using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.SD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientOpcUaTiaPortal.ListbBoxMainApplication
{
    public class CreateDbBlocks
    {
        private List<dbBlock> _dbBlocks;
        public listObjectType listObjectType { get; }
        private ItemIntoDbBlocks _itemIntoDbBlocks;
        public CreateDbBlocks(List<dbBlock> dbBlocks,ItemIntoDbBlocks itemIntoDbBlocks)
        {
            _dbBlocks = dbBlocks;
            _itemIntoDbBlocks = itemIntoDbBlocks;
            listObjectType = _itemIntoDbBlocks.listObjectType;
        }

        public void AddDbBlocks(dbBlock db, ListBox list, List<object> listItems)
        {
            Grid grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(50, 0, 0, 0),
                Width = 650,
                Background = System.Windows.Media.Brushes.AliceBlue
            };

            // Define Row Definitions
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Define Column Definitions
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            // Add TextBlock (NameDb)
            TextBlock nameDbTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "DB name: " + db.NameDb // Bind NameDb from the passed nazwaDB object
            };
            Grid.SetRow(nameDbTextBlock, 0);
            Grid.SetColumn(nameDbTextBlock, 1);
            grid.Children.Add(nameDbTextBlock);

            // Add Labels
            grid.Children.Add(CreateTextBlock("Variable name:", 1, 0));
            grid.Children.Add(CreateTextBlock("Varaible type:", 1, 1));
            grid.Children.Add(CreateTextBlock("Value:", 1, 2));

            // Add TextBox (nazwaZm)
            TextBox nazwaZmTextBox = new TextBox
            {
                Name = "nazwaZm"
            };
            Grid.SetRow(nazwaZmTextBox, 2);
            Grid.SetColumn(nazwaZmTextBox, 0);
            grid.Children.Add(nazwaZmTextBox);

            // Add ComboBox (type1Var)
            ComboBox type1VarComboBox = new ComboBox
            {
                Name = "type1Var",
            };
            type1VarComboBox.SelectionChanged += (sender, e) => ComboBoxEvent.ChangeItemInCombobox(sender, listObjectType);
            type1VarComboBox.DropDownOpened += (sender, e) => ComboBoxEvent.DropDownInComboBox(sender, listObjectType);
            Grid.SetRow(type1VarComboBox, 2);
            Grid.SetColumn(type1VarComboBox, 1);
            grid.Children.Add(type1VarComboBox);

            // Add TextBox (wartoscZm)
            TextBox wartoscZmTextBox = new TextBox
            {
                Name = "wartoscZm"
            };
            Grid.SetRow(wartoscZmTextBox, 2);
            Grid.SetColumn(wartoscZmTextBox, 2);
            grid.Children.Add(wartoscZmTextBox);

            // Add Button (addWart)
            Button addWartButton = new Button
            {
                Name = "addWart",
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 60,
                Content = "Add"
            };
            addWartButton.Click += (sender, e) =>
            {
                Button btn = sender as Button;
                dbBlock nazwaDB = btn.DataContext as dbBlock;
                _itemIntoDbBlocks.AddEvent(sender, nazwaDB);
            };//button to add variable
            Grid.SetRow(addWartButton, 2);
            Grid.SetColumn(addWartButton, 3);
            grid.Children.Add(addWartButton);

            // Add ListBox (listVar)
            ListBox listVarListBox = new ListBox
            {
                Name = "listVar",
            };
            if (listItems != null)
            {
                foreach (var item in listItems)
                {
                    listVarListBox.Items.Add(item);
                }
            }
            Grid.SetRow(listVarListBox, 4);
            Grid.SetColumnSpan(listVarListBox, 4);
            grid.Children.Add(listVarListBox);
            grid.DataContext = db;

            // Add the created Grid to the ListBox (dbBlock)
            list.Items.Add(grid);
        }
        private TextBlock CreateTextBlock(string text, int row, int column)//to delete
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text
            };
            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            return textBlock;
        }

    }
}
