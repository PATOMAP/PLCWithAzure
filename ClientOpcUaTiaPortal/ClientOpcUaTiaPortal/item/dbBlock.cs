﻿using System.Windows;
using System.Windows.Documents;

namespace ClientOpcUaTiaPortal.item
{


    public class dbBlock//klasa do nowego item w listbox
    {
        public int ind { get; set; }
        public string NameDb { get; set; }


        public List<listType> listType1 { get; set; }

        public List<itemDB> _itemsDB { get; set; }

        public dbBlock(List<listType> _listType)
        {
            _itemsDB= new List<itemDB>();//zmiana ostatnia
            listType1=new List<listType>();
            foreach (listType item in _listType)
            {
                listType1.Add(item);
            }
            
        }

        public void addItemIntoListBox(itemDB itemDB)
        {
            if (_itemsDB == null)
            {
                _itemsDB = new List<itemDB>();
            }
            _itemsDB.Add(itemDB);

        }


    }

}