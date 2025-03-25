using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientOpcUaTiaPortal.item
{
    public class SendToStructWindow
    {
        public string name { get; set; }//nazwa struktury
        public bool child { get; set; }// które dziecko
        public ListBox boxx { get; set; }
        public int ind { get; set; }
    }
}
