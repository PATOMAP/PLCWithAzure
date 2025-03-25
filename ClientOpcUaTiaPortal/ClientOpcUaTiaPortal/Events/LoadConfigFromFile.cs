using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.ListbBoxMainApplication;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientOpcUaTiaPortal.Events
{
    public  class LoadConfigFromFile
    {
    //to save data

        public static List<dbBlock> Load(List<dbBlock> dbBlocks, CreateDbBlocks functionDB, ListBox listBoxDB,EventWithServer eventWithServer)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "Wszystkie pliki (*.*)|*.*";
            List<string> text;
            List<itemFromConfig> itemsString = new List<itemFromConfig>(); ;
            if (openFileDialog.ShowDialog() == true)
            {

                string selectedFilePath = openFileDialog.FileName;
                text = new List<string>(File.ReadAllLines(selectedFilePath));
                sliceTextFromTxt(text, itemsString);

                listBoxDB.Items.Clear();
                dbBlocks.Clear();
                crateBlockListBox(dbBlocks, functionDB, itemsString, listBoxDB,eventWithServer);

            }
            return dbBlocks;

        }
        private static void sliceTextFromTxt(List<string> text, List<itemFromConfig> itemsString)
        {
            
            foreach (string s in text)
            {
                itemFromConfig item = new itemFromConfig();
                string[] parts = s.Split(';');
                item.type1 = parts[1];
                item.Value = parts[2];
                item.arrayAct = Convert.ToBoolean(parts[3]);
                item.structAct = Convert.ToBoolean(parts[4]);

                string[] nameParts = parts[0].Split(".");

                item.nameData = nameParts[0];

                for (int i = 1; i < nameParts.Length; i++)
                    item.nameVar.Add(nameParts[i]);

                itemsString.Add(item);
            }


        }
        private static void crateBlockListBox(List<dbBlock> dbBlocks, CreateDbBlocks functionDB, 
            List<itemFromConfig> itemsString, 
            ListBox listBoxDB, 
            EventWithServer eventWithServer)
        {
            int licz = 0;
            string nameDataBase = "";
            dbBlock itemDB = new dbBlock(functionDB.listObjectType._list);
            List<object> listItems;

            for (int i = 0; i < itemsString.Count; i++)
            {

                if (nameDataBase != itemsString[i].nameData)
                {
                    if (nameDataBase != "")
                    {
                        dbBlocks.Add(itemDB);
                        listItems = AddToListBox.AddL(dbBlocks[dbBlocks.Count - 1]._itemsDB,eventWithServer);//addItemsToListBox
                        functionDB.AddDbBlocks(dbBlocks[dbBlocks.Count - 1], listBoxDB, listItems);

                    }


                    itemDB = new dbBlock(functionDB.listObjectType._list) { NameDb = itemsString[i].nameData, ind = licz++ };
                    nameDataBase = itemsString[i].nameData;
                }
                addItemIntoDB(itemDB._itemsDB, itemsString[i], 0);


            }

            dbBlocks.Add(itemDB);
            listItems = AddToListBox.AddL(dbBlocks[dbBlocks.Count - 1]._itemsDB, eventWithServer);//addItemsToListBox
            functionDB.AddDbBlocks(dbBlocks[dbBlocks.Count - 1], listBoxDB, listItems);

        }
        private static void addItemIntoDB(List<itemDB> items, itemFromConfig itemConf, int poz)
        {

            if (poz == itemConf.nameVar.Count - 1)
            {
                items.Add(new itemDB { Name = itemConf.nameVar[poz], Value = itemConf.Value, type1 = itemConf.type1, arrayAct = itemConf.arrayAct, structAct = itemConf.structAct });
            }
            else
            {
                foreach (itemDB itemDB in items)
                {
                    if (itemDB.Name == itemConf.nameVar[poz])
                    {
                        addItemIntoDB(itemDB._item, itemConf, ++poz);
                        break;
                    }
                }
            }

        }


    }
}
