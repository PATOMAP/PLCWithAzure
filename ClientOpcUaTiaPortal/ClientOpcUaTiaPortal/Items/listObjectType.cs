using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ClientOpcUaTiaPortal.item
{
    public class listObjectType
    {
       public List<ItemsToComobBox> _list;
        public listObjectType() 
        {
            _list = new List<ItemsToComobBox>();
            _list.Add(new ItemsToComobBox { Name = "Array" });
            _list.Add(new ItemsToComobBox { Name = "Struct" });
            _list.Add(new ItemsToComobBox { Name = "Int16" });
            _list.Add(new ItemsToComobBox { Name = "String" });
            _list.Add(new ItemsToComobBox { Name = "Real" });
            _list.Add(new ItemsToComobBox { Name = "Bool" });
            _list.Add(new ItemsToComobBox { Name = "Byte" });
            _list.Add(new ItemsToComobBox { Name = "SByte" });
            _list.Add(new ItemsToComobBox { Name = "UInt16" });
            _list.Add(new ItemsToComobBox { Name = "Int32" });
            _list.Add(new ItemsToComobBox { Name = "UInt32" });
            _list.Add(new ItemsToComobBox { Name = "Int64" });
            _list.Add(new ItemsToComobBox { Name = "UInt64" });
            _list.Add(new ItemsToComobBox { Name = "Real" });
            _list.Add(new ItemsToComobBox { Name = "Double" });

            
        }
        public bool addItem(string name,itemDB item)
        {
            if(_list.Any(x => x.Name == name))
            {
                MessageBox.Show("Istnieje taki typ");
                return true;
            }
            if(item ==null)
                _list.Add(new ItemsToComobBox { Name = name });
            else 
                _list.Add(new ItemsToComobBox {Name = name,_itemDB=item});
            return false;
        }
    }
}
