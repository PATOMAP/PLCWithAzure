using ClientOpcUaTiaPortal.item;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientOpcUaTiaPortal.Events
{
    public class SaveConfig
    {
        public static void Save(List<dbBlock> dbBlocks)//to modify
        {
            StringBuilder config = new StringBuilder();
            foreach (dbBlock nazwa in dbBlocks)
            {

                createNameToFile(nazwa.NameDb, nazwa._itemsDB, config);
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "Wszystkie pliki (*.*)|*.*";
            string dilePath;
            if (openFileDialog.ShowDialog() == true)
            {

                dilePath = openFileDialog.FileName;

            }
            else
                return;
            File.WriteAllText(dilePath, config.ToString());

        }

        private static void createNameToFile(string name, List<itemDB> itemsDB, StringBuilder config)
        {
            foreach (itemDB itemDB in itemsDB)
            {
                config
                    .Append(name + "." + itemDB.Name + ";" + itemDB.type1 + ";" + itemDB.Value + ';' + itemDB.arrayAct.ToString() + ';' + itemDB.structAct.ToString())
                    .AppendLine();

                if (itemDB._item.Count != 0)
                {
                    createNameToFile(name + "." + itemDB.Name, itemDB._item, config);
                }
            }

        }
    }
}
