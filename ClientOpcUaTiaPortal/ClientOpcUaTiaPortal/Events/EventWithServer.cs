using ClientOpcUaTiaPortal.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClientOpcUaTiaPortal.Events
{

    public class EventWithServer
    {
        private ConnectOpcUa serwerOpcUa;
        private DispatcherTimer typingTimer;
        private const int TypingDelay = 1000; // Opóźnienie w milisekundach
        private itemDB _itemToWrite;//item must to write
        bool statCon = false;
        private List<dbBlock> _dbBlocks;
        private TextBox _urlServer;
        public EventWithServer(TextBox urlServer, List<dbBlock> dbBlocks)
        {
            typingTimer = new DispatcherTimer();
            typingTimer.Interval = TimeSpan.FromMilliseconds(TypingDelay);
            typingTimer.Tick += TypingTimer_Tick;
            _dbBlocks = dbBlocks;
            _urlServer = urlServer;
        }

        private void TypingTimer_Tick(object sender, EventArgs e)//write value to opc ua
        {

            typingTimer.Stop();
            if (serwerOpcUa != null)
            {
                serwerOpcUa.Write_Value(_itemToWrite);
            }

        }

        public void ChangeTextInValue(object sender, KeyEventArgs e)//delay to write
        {
            if (e.Key == Key.Enter)
            {
                TextBox textChnaged = sender as TextBox;
                dbBlock dbTemp = textChnaged.DataContext as dbBlock;
                CreateNameToDatabasePath.Create("\"" + dbTemp.NameDb + "\".", dbTemp._itemsDB, new StringBuilder());
                var bindingExpression = textChnaged.GetBindingExpression(TextBox.TextProperty);
                _itemToWrite = bindingExpression?.ResolvedSource as itemDB;
                _itemToWrite.Value = textChnaged.Text;
                _itemToWrite._readWrite = 'w';

                typingTimer.Stop();
                typingTimer.Start();
            }

        }
        public void ConnectWithOpcUa(object sender)
        {
            Button btn = sender as Button;
            if (!statCon)
            {
                btn.Content = "Disconnect";
                serwerOpcUa = new ConnectOpcUa(_dbBlocks, _urlServer.Text);
            }

            else
            {
                btn.Content = "Connect";
                serwerOpcUa.DisconnectFromOpcUa();
            }
            statCon = !statCon;

        }
        public void ReadWithOpcUa(object sender)
        {
            Button btn = sender as Button;
            if (serwerOpcUa == null)
            {
                MessageBox.Show("Nie połączyłeś się ze serwerem!");
                return;
            }
            serwerOpcUa.changeClock(btn);
        }
    }
}
