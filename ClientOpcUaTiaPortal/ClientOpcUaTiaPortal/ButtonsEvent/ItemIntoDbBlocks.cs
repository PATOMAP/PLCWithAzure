using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.SD;
using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ClientOpcUaTiaPortal.Events;
using ClientOpcUaTiaPortal.CheckClass;

namespace ClientOpcUaTiaPortal.ListbBoxMainApplication
{
    public  class ItemIntoDbBlocks
    {
        public EventWithServer eventWithServer { get; }
        public listObjectType listObjectType { get; }
        private itemDB structItem;//global struct
        private itemDB arrayItem;//global array
        public ItemIntoDbBlocks(EventWithServer eventWithServer, listObjectType listObjectType)
        {
             this.eventWithServer=eventWithServer;
             this.listObjectType =listObjectType;
        }

        public itemDB AddEvent(object sender, dbBlock _nazwaDB)//add item into DB
        {

            var btn = sender as Button;
            Grid grid = btn.Parent as Grid;
            grid.Tag = btn.Tag;
            UIElementCollection items = grid.Children as UIElementCollection;// 



            TextBox nazwa = items[4] as TextBox;
            ComboBox combo = items[5] as ComboBox;

            string choiceItem = combo.SelectedItem as string; //as listType;
            string choice = choiceItem;
            TextBox wart = items[6] as TextBox;
            ListBox boxxx = items[8] as ListBox;

            dbBlock dBAct = combo.DataContext as dbBlock;
            int ind = dBAct.ind;

            if (grid.Tag == null)
            {
                dbBlock db = grid.DataContext as dbBlock;
                if (findTheSameName(db._itemsDB, nazwa.Text.ToString()) == true || (ConvertValueFromPLC.ConvertVar(choice, wart.Text.ToString()) == null && CheckIsStruct.Check(choice,listObjectType)))
                {
                    if (findTheSameName(db._itemsDB, nazwa.Text.ToString()) == true)
                        MessageBox.Show("Zmienna jest już w bazie");
                    else
                        MessageBox.Show("Zły typ");

                    return null;
                }


            }
            else
            {
                itemDB db = grid.DataContext as itemDB;
                if (findTheSameName(db._item, nazwa.Text.ToString()) == true || (ConvertValueFromPLC.ConvertVar(choice, wart.Text.ToString()) == null && CheckIsStruct.Check(choice, listObjectType)))
                {
                    if (findTheSameName(db._item, nazwa.Text.ToString()) == true)
                        MessageBox.Show("Zmienna jest już w bazie");
                    else
                        MessageBox.Show("Zły typ");
                    return null;
                }

            }

            if (choice == "Struct")
            {
                SendToStructWindow sendToStruct = new SendToStructWindow();
                sendToStruct.name = nazwa.Text.ToString();
                sendToStruct.boxx = boxxx;

                if (grid.Tag == null)//sprawdzenie czy strutura nie ma nad sob innej struktury
                    sendToStruct.child = false;
                else
                    sendToStruct.child = true;

                StructWindow structWindow = new StructWindow(_nazwaDB, this, sendToStruct);
                structWindow.DataSent += OnDataReceivedStruct;
                bool? dialogResult = structWindow.ShowDialog(); // Otwórz jako modalne okno

                if (structItem != null && structItem.structAct == true)
                {
                    return structItem;
                }
                return new itemDB();
            }
            else if (choice == "Array")
            {
                ArrayWindow arrayWindow = new ArrayWindow(_nazwaDB, grid, listObjectType._list);
                arrayWindow.DataSent += OnDataReceivedArray;
                bool? dialogResult = arrayWindow.ShowDialog();
                if (dialogResult == false)
                {
                    return arrayItem;
                }
                return new itemDB();
            }
            else
            {
                listType type = null;
                foreach (var item in listObjectType._list)
                {
                    if (item.Name == choice)
                    {
                        type = item;
                        break;
                    }

                }
                itemDB temp = null;
                if (type._itemDB == null)
                {
                    temp = new itemDB { Name = nazwa.Text.ToString(), type1 = choice, Value = wart.Text.ToString(), arrayAct = false, structAct = false };
                    Grid newGrid;
                    if (grid.Tag == null)
                    {
                        _nazwaDB.addItemIntoListBox(temp);
                        newGrid = GridClass.CreateDynamicGrid(_nazwaDB._itemsDB[_nazwaDB._itemsDB.Count - 1], eventWithServer);
                    }
                    else
                    {
                        newGrid = GridClass.CreateDynamicGrid(temp,eventWithServer);
                    }

                    boxxx.Items.Add(newGrid);

                }
                else
                {
                    type._itemDB.Name = nazwa.Text.ToString();
                    temp = type._itemDB;
                    ListBox listToAdd = new ListBox();
                    if (grid.Tag == null)
                        _nazwaDB.addItemIntoListBox(temp);

                    List<object> listobj = AddToListBox.AddL(temp._item,eventWithServer);
                    foreach (var item in listobj)
                    {
                        listToAdd.Items.Add(item);
                    }
                    Expander ex = new Expander();
                    ex.Header = GridClass.AddExpanderToStructOrArray(nazwa.Text.ToString());
                    ex.Content = listToAdd;
                    boxxx.Items.Add(ex);
                }
                return temp;
            }

        }
        public bool findTheSameName(List<itemDB> items, string name)
        {

            if (items != null)
                foreach (var item in items)
                {
                    if (item.Name == name)
                    {

                        return true;
                    }

                }

            return false;

        }
        private void OnDataReceivedArray(object sender, sendData data)//pobrane dane z array
        {
            var ob = data.grid.Tag;
            UIElementCollection items = data.grid.Children as UIElementCollection;// 
            TextBox nazwa = items[4] as TextBox;
            ListBox boxxx = items[8] as ListBox;

            Expander ex = new Expander();

            ex.Header = GridClass.AddExpanderToStructOrArray(nazwa.Text.ToString());
            StackPanel stackArray = new StackPanel();


            if (ob != null)
            {
                arrayItem = new itemDB { Name = nazwa.Text.ToString(), arrayAct = true };
            }
            else
            {
                itemDB itemMain = new itemDB { Name = nazwa.Text.ToString(), arrayAct = true, _item = new List<itemDB>(), structAct = false };
                data._nazwaDB.addItemIntoListBox(itemMain);
            }

            for (int i = 0; i < data.count; i++)
            {
                itemDB temp = new itemDB { Name = nazwa.Text.ToString() + "[" + i.ToString() + "]", type1 = data.type, Value = "0", arrayAct = true, structAct = false };//arrayAct=false

                if (ob == null)
                    data._nazwaDB._itemsDB[data._nazwaDB._itemsDB.Count - 1]._item.Add(temp);//dodanie elementu do listy wewnątzr tej samej klasy
                else
                    arrayItem._item.Add(temp);

                Grid newGrid = GridClass.CreateDynamicGrid(temp,eventWithServer);
                stackArray.Children.Add(newGrid);
            }

            ex.Content = stackArray;//dodanie elementu do expandera 
            boxxx.Items.Add(ex);
        }
        private void OnDataReceivedStruct(object sender, SendDataStruct data)//pobrane dane z struct
        {

            if (data.child == false)//zmiany
            {

                string name = data.itemDB.Name;
                data.itemDB.Name = "";
                if (listObjectType.addItem(name, data.itemDB))
                    return;

                data._nazwaDB.listType1 = listObjectType._list;



            }
            else
            {
                structItem = data.itemDB;
                structItem.structAct = true;
            }
        }
    }
}
