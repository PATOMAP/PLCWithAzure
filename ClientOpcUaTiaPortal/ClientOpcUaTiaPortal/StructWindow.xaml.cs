using ClientOpcUaTiaPortal;
using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClientOpcUaTiaPortal.ListbBoxMainApplication;
using ClientOpcUaTiaPortal.SD;

namespace DB
{
    /// <summary>
    /// Interaction logic for StructWindow.xaml
    /// </summary>
    public partial class StructWindow : Window
    {
        public event EventHandler <SendDataStruct> DataSent;
        
        dbBlock structDB;
        itemDB _itemDB;
        SendToStructWindow _inf;
        ItemIntoDbBlocks _itemIntoDbBlocks;
        public StructWindow(dbBlock list, ItemIntoDbBlocks itemIntoDbBlocks, SendToStructWindow information)
        {
            _inf = information;
            _itemDB = new itemDB { Name = _inf.name};
            structDB = list;
           _itemIntoDbBlocks = itemIntoDbBlocks;
            this.DataContext = _itemDB;
            
            
            InitializeComponent();
            type1Var.DataContext = structDB;
            structNamee.Text = _inf.name;
        }

        private void type1Var_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox button = (ComboBox)sender;
            button.Tag = "zm";
            //_windowFunc.type1Var_SelectionChanged(button, e);
            ComboBoxEvent.ChangeItemInCombobox(button,_itemIntoDbBlocks.listObjectType);

        }

        private void addWart_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.Tag = "zm";
            // itemDB ret = _windowFunc.addEvent(button,structDB);
            itemDB ret = _itemIntoDbBlocks.AddEvent(sender,structDB);
            if (ret == null)
                return;
            _itemDB._item.Add(ret);
            if(ret.structAct==true ) 
            {
                Expander ex=createExpander(ret);
                listVar.Items.Add(ex);
            }
        }
        private Expander createExpander(itemDB item)
        {
            Expander expander= new Expander();
            ListBox listBox = listBoxToSend(item);
            //expander.Header = _windowFunc.createGridExpander(item.Name);
            expander.Header = GridClass.AddExpanderToStructOrArray(item.Name);
            expander.Content = listBox;
            return expander;    

        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {

            if (_itemDB._item.Count == 0)
                return;

            SendDataStruct sendStruct = new SendDataStruct { itemDB = _itemDB,boxx=_inf.boxx,child=_inf.child,ind=structDB.ind,_nazwaDB=structDB };

            //sendStruct.expanderSend = createExpander(_itemDB);

            DataSent?.Invoke(this,sendStruct);
            this.DialogResult = true;
        }
         
        private ListBox listBoxToSend(itemDB itemDB)
        {
            ListBox listboxToSend = new ListBox();
            foreach (itemDB item in itemDB._item)
            {
                if(item._item.Count > 0)
                {
                    Expander expander = new Expander();
                    // expander.Header = _windowFunc.createGridExpander(item.Name);
                    expander.Header = GridClass.AddExpanderToStructOrArray(item.Name);

                    ListBox listret= listBoxToSend(item);
                    expander.Content = listret;
                    listboxToSend.Items.Add(expander);
                }
                else
                {
                    //Grid grid= _windowFunc.CreateDynamicGrid(item);
                    Grid grid = GridClass.CreateDynamicGrid(item,_itemIntoDbBlocks.eventWithServer);
                    listboxToSend.Items.Add(grid);
                }

            }
            return listboxToSend;
        }

        private void type1Var_DropDownOpened(object sender, EventArgs e)
        {
            //_windowFunc.comboBox1_DropDown(sender, e);
            ComboBoxEvent.DropDownInComboBox(sender, _itemIntoDbBlocks.listObjectType);
        }
    }
}
