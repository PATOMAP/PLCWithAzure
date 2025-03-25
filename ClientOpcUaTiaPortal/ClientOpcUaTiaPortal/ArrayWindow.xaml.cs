using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using ClientOpcUaTiaPortal.item;


namespace DB
{

    /// <summary>
    /// Interaction logic for ArrayWindow.xaml
    /// </summary>

    public partial class ArrayWindow : Window
    {
        public event EventHandler <sendData> DataSent;

        private dbBlock _nazwaDB;
        private dbBlock _nazwaDBOrg;
        private Grid _grid;

        public ArrayWindow(dbBlock dbPrz,Grid grid,List<listType>list)
        {
            _nazwaDBOrg=dbPrz;
            _nazwaDB = new dbBlock(list){ind=dbPrz.ind};
            _grid = grid;
            removeListComobo(_nazwaDB);
            this.DataContext = _nazwaDB;
            InitializeComponent();
        }

        public void removeListComobo(dbBlock DB)
        {

            DB.listType1.RemoveAll(p => p.Name == "Array");
            DB.listType1.RemoveAll(p => p.Name == "Struct");
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            int roz;
            string choice;

            try
            {
                 roz=Convert.ToInt16( sizeArray.Text.ToString()); 
                 choice = (arrayBox.SelectedItem as listType).Name.ToString();
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233033)
                    MessageBox.Show("Nie wpisałeś liczby do wielkości!!!!");

                else if (ex.HResult == -2147467261)
                    MessageBox.Show("Wybierz format!!!!");
                else
                    MessageBox.Show("Nieznany błąd");

                e.Cancel = true;
                return;
            }
            sendData dataTemp = new sendData {count=roz,type=choice,ind=_nazwaDB.ind,grid=_grid,_nazwaDB=_nazwaDBOrg};
            
            DataSent?.Invoke(this,dataTemp);
        }

    }
}
