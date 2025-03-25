using System.Windows;
using System.Windows.Controls;
using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.ListbBoxMainApplication;
using ClientOpcUaTiaPortal.Events;
//opc.tcp://192.168.0.200:4840
namespace ClientOpcUaTiaPortal
{
    /// <summary>
    /// Interaction logic for CreateConfig.xaml
    /// </summary>
    public partial class CreateConfig : UserControl
    {

        ContentControl _contentControl;
        private List<dbBlock> _dbBlocks;
        listObjectType _listObjectType;//combobox to select value
        private CreateDbBlocks _functionDB;
        private EventWithServer _eventWithServer;
        private ItemIntoDbBlocks _itemIntoDbBlocks;

        public CreateConfig(ContentControl sendControl)
        {

            InitializeComponent();
            _listObjectType = new listObjectType();
            _dbBlocks = new List<dbBlock>();
            _eventWithServer = new EventWithServer(urlSerwer, _dbBlocks);
            _itemIntoDbBlocks = new ItemIntoDbBlocks(_eventWithServer, _listObjectType);
            _functionDB = new CreateDbBlocks(_dbBlocks, _itemIntoDbBlocks);

            _contentControl = sendControl;//content widow
        }

        private void addDB(object sender, RoutedEventArgs e)//object xaml
        {
            var ob = new dbBlock (_listObjectType._list){ NameDb =   nazwaDb.Text.ToString(), ind = _dbBlocks.Count };
            _dbBlocks.Add(ob);
            _functionDB.AddDbBlocks(_dbBlocks[_dbBlocks.Count - 1], listBoxDB, null);
           
        }



        private void SaveData_Click(object sender, RoutedEventArgs e)//to modify
        {
            SaveConfig.Save(_dbBlocks);

        }



        private void LoadConfig(object sender, RoutedEventArgs e)
        {
            _dbBlocks= LoadConfigFromFile.Load(_dbBlocks, _functionDB, listBoxDB, _eventWithServer);

        }

        private void connectWithOpcUa(object sender, RoutedEventArgs e)
        {
            _eventWithServer.ConnectWithOpcUa(sender);

        }

        private void readWithOpcUa(object sender, RoutedEventArgs e)
        {
            _eventWithServer.ReadWithOpcUa(sender);

        }

    }
}
