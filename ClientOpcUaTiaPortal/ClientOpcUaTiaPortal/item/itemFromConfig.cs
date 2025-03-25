using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOpcUaTiaPortal.item
{
    public class itemFromConfig:itemDB
    {
        public string nameData {  get; set; }
        public List<string> nameVar { get; set; }
   
        public itemFromConfig() 
        {
            nameVar = new List<string>();
        }
    }
}
