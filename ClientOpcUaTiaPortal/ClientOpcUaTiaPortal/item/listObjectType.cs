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
       public List<listType> _list;
        public listObjectType() 
        {
            _list = new List<listType>();
            _list.Add(new listType { Name = "Array" });
            _list.Add(new listType { Name = "Struct" });
            _list.Add(new listType { Name = "Int16" });
            _list.Add(new listType { Name = "String" });
            _list.Add(new listType { Name = "Real" });
            _list.Add(new listType { Name = "Bool" });
            _list.Add(new listType { Name = "Byte" });
            _list.Add(new listType { Name = "SByte" });
            _list.Add(new listType { Name = "UInt16" });
            _list.Add(new listType { Name = "Int32" });
            _list.Add(new listType { Name = "UInt32" });
            _list.Add(new listType { Name = "Int64" });
            _list.Add(new listType { Name = "UInt64" });
            _list.Add(new listType { Name = "Real" });
            _list.Add(new listType { Name = "Double" });

            
        }
        public bool addItem(string name,itemDB item)
        {
            if(_list.Any(x => x.Name == name))
            {
                MessageBox.Show("Istnieje taki typ");
                return true;
            }
            if(item ==null)
                _list.Add(new listType { Name = name });
            else 
                _list.Add(new listType {Name = name,_itemDB=item});
            return false;
        }
    }
}
