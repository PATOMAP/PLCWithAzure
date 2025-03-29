using System.Windows.Threading;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua;
using System.Windows;
using System.Windows.Controls;
using ClientOpcUaTiaPortal.SD;
using ClientOpcUaTiaPortal.TransferInformation;

namespace ClientOpcUaTiaPortal.item
{
    public class ConnectOpcUa
    {
        static SessionReconnectHandler? m_reconnectHandler;
        static Session? m_session;
        private DispatcherTimer _timer;
        private bool _stat;

        private List<dbBlock> dbBlocks;
        private Button btn;

        private List<ItemFromInflux> _itemFromInflux;
        private List<ItemInflux> _zmList;//azure function

        private ConnectWithInflux _connectWithInflux;
        private float setpoint;
        private bool statusZap;
        private bool statusOnce;
        private DateTime timeStart;

        public ConnectOpcUa(List<dbBlock> nazwaDBs,string name) 
        {
            _zmList = new List<ItemInflux>();
            _stat = false;
            dbBlocks = nazwaDBs;
            _itemFromInflux = new List<ItemFromInflux>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);//TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            InitializeOpcUaClient(name);
            _connectWithInflux= new ConnectWithInflux(_zmList,_itemFromInflux);

        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (m_session == null)
            {
                MessageBox.Show("Server disconnected");
                changeClock(null);
                return;
            }
            foreach (dbBlock db in dbBlocks)
            {
                ReadFromOpcua(db._itemsDB, "\"" + db.NameDb + "\".");
            }

        }


        public void changeClock(Button _btn)
        {
            if(_btn != null)
            {
                btn = _btn;
                _stat = !_stat;
            }
            else
            {
                _stat = false;

            }
            


            if (_stat)
            {
                btn.Content = "Stop";
                _timer.Start();           
            }

            else
            {
                _timer.Stop();
                if(btn!= null) 
                btn.Content = "Read";
            }

        }

        public void DisconnectFromOpcUa()
        {

            changeClock(null);
            if (m_session != null)
            {
                m_session.Close();
            }
            m_session = null;

        }

        private async void InitializeOpcUaClient(string url)
        {

            await downloadfromOpcua(url);

        }
        private static async Task downloadfromOpcua(string url)
        {
            ApplicationInstance application = new ApplicationInstance();

            application.ApplicationType = Opc.Ua.ApplicationType.Client;
            application.ConfigSectionName = "Client";
            application.LoadApplicationConfiguration(false).Wait();
            application.CheckApplicationInstanceCertificate(false, 0).Wait();
            var m_configuration = application.ApplicationConfiguration;
            m_configuration.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
            string serverUrl = url;

            try
            {
                var endpointDescription = CoreClientUtils.SelectEndpoint(m_configuration, serverUrl, true, 30000);
                var endpointConfiguration = EndpointConfiguration.Create(m_configuration);
                var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

                m_session = await Session.Create(
              m_configuration,
              endpoint,
              false,
              true,  
            m_configuration.ApplicationName,  600000, null, null);
            }
            catch
            {
                MessageBox.Show("Error connection");
                return;
            }


            m_session.KeepAlive += Session_KeepAlive;
            m_reconnectHandler = new SessionReconnectHandler(true, 100 * 1000);//10 * 1000



          
        }


        private static void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (ServiceResult.IsBad(e.Status))
            {
                m_reconnectHandler.BeginReconnect(m_session, 100000, Server_ReconnectComplete);//1000
            }
        }
        private static void Server_ReconnectComplete(object sender, EventArgs e)
        {
            if (m_reconnectHandler.Session != null)
            {
                if (!ReferenceEquals(m_session, m_reconnectHandler.Session))
                {
                    var session = m_session;
                    session.KeepAlive -= Session_KeepAlive;
                    m_session = m_reconnectHandler.Session as Session;
                    m_session.KeepAlive += Session_KeepAlive;
                    Utils.SilentDispose(session);
                }
            }
        }
        private static void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        private void ReadFromOpcua(List<itemDB> items, string name)
        {
            _connectWithInflux.CheckChangeInDataBase(statusZap, statusOnce, setpoint);
            foreach (var itemDB in items)
            {
                if (_itemFromInflux.Any(p => p.Name == itemDB.Name))
                {
                    ItemFromInflux objRem = _itemFromInflux.FirstOrDefault(p => p.Name == itemDB.Name);
                    itemDB.Path = name + "\"" + itemDB.Name + "\"";
                    itemDB.Value = Convert.ToString(objRem.Value);
                    Write_Value(itemDB);

                    _itemFromInflux.Remove(objRem);
                }
                else if (itemDB._item.Count == 0 && itemDB._readWrite == 'r')
                {
                    string readNode = "ns=3;s=" + name + "\"" + itemDB.Name + "\"";
                    ReadValueId nodeToRead = new ReadValueId();//przeniesione
                    nodeToRead.NodeId = new NodeId(readNode);
                    nodeToRead.AttributeId = Attributes.Value;

                    ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
                    nodesToRead.Add(nodeToRead);

                    DataValueCollection readResults = null;
                    DiagnosticInfoCollection readDiagnosticInfos = null;

                    ResponseHeader readHeader = m_session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out readResults, out readDiagnosticInfos);

                    ClientBase.ValidateResponse(readResults, nodesToRead);
                    ClientBase.ValidateDiagnosticInfos(readDiagnosticInfos, nodesToRead);

                    var m_value = readResults[0];
                    itemDB.Value = m_value.ToString();
                    if (itemDB.Name == "SetPoint")
                    {
                        float valueD = Convert.ToSingle(itemDB.Value);
                        if (statusOnce == false && valueD != 0)
                        {
                            statusZap = true;
                            statusOnce = true;
                            setpoint = valueD;
                        }
                        else if (statusOnce == true && setpoint != valueD)
                        {
                            setpoint = valueD;

                            statusZap = true;

                            _zmList.Clear();
                        }

                    }

                    if (itemDB.Name == "y" && name == "\"PID_Data\"." && statusZap == true)
                    {

                        if (_zmList.Count == 0)
                        {
                            timeStart = DateTime.Now;
                            _zmList.Add(new ItemInflux { time = TimeSpan.Zero, ValueF = Convert.ToSingle(itemDB.Value), Setpoint = setpoint });//tempItemInflux 
                        }
                        else
                            _zmList.Add(new ItemInflux { time = DateTime.Now - timeStart, ValueF = Convert.ToSingle(itemDB.Value), Setpoint = setpoint });//tempItemInflux 

                    }

                    _connectWithInflux.WriteToDatabase(itemDB, name, timeStart, statusZap);
                }
                else
                {

                    ReadFromOpcua(itemDB._item, name + "\"" + itemDB.Name + "\".");
                }

            }


        }

        public void Write_Value(itemDB item)
        {

          
            WriteValue valueToWrite = new WriteValue();
            valueToWrite.NodeId = new NodeId("ns=3;s="+item.Path);
            valueToWrite.AttributeId = Attributes.Value;
            try
            {
                
                valueToWrite.Value.Value=ConvertValueFromPLC.ConvertVar(item.type1, item.Value);
                if (valueToWrite.Value.Value == null)
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Wrong format!");
                return;
            }

            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;

            WriteValueCollection valuesToWrite = new WriteValueCollection();
            valuesToWrite.Add(valueToWrite);

            StatusCodeCollection writeResults = null;
            DiagnosticInfoCollection writeDiagnosticInfos = null;

            ResponseHeader writeHeader = m_session.Write(null, valuesToWrite, out writeResults, out writeDiagnosticInfos);

            item._readWrite = 'r';
            
        }

    }
}
