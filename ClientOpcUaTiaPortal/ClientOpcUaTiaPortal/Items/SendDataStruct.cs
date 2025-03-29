using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientOpcUaTiaPortal.item
{
    public class SendDataStruct : SendData
    {
        public Expander expanderSend { get; set; }


        public itemDB itemDB { get; set; }

        public ListBox boxx { get; set; }
        public SendDataStruct()
        {
            expanderSend = new Expander();
        }

    }
}
