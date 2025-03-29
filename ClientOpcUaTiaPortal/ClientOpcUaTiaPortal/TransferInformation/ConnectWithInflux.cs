using ClientOpcUaTiaPortal.item;
using ClientOpcUaTiaPortal.SD;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ClientOpcUaTiaPortal.TransferInformation
{
    public class ConnectWithInflux
    {
        private string _token;
        private string _bucket;
        private string _org;
        private object _client;
        List<ItemInflux> _zmList;
        List<ItemFromInflux> _itemFromInflux;
        public ConnectWithInflux(List<ItemInflux> zmList, List<ItemFromInflux> itemFromInflux)
        {
            _zmList = zmList;
            _itemFromInflux = itemFromInflux;
            _token = ConfigurationManager.ConnectionStrings["token"].ConnectionString;
            _bucket = "dataopcua";
            _org = "opcua";
            _client = new InfluxDBClient("http://localhost:8086", _token);//when we host we susing url from azure
        }
        public void WriteToDatabase(itemDB item, string name,DateTime timeStart, bool statusZap)//new class
        {
            InfluxDBClient client = (InfluxDBClient)_client;
            string meas;
            if (name == "\"Simulation_Data\".")
            {
                meas = "mem1";
            }
            else if (name == "\"Comparison_Data\"." || name == "\"Real_Data\".")
            {
                meas = "mem2";
            }
            else
            {
                meas = "mem3";
            }


            if (item.type1 != "String" && item.type1 != "Bool")
            {
                var zm = ConvertValueFromPLC.ConvertVar(item.type1, item.Value);
                var point = PointData
               .Measurement(meas)
               .Tag("host", "host1")
               .Field(item.Name, zm)
               .Timestamp(DateTime.Now, WritePrecision.Ns);
                Console.WriteLine(DateTime.Now);

                using (var writeApi = client.GetWriteApi())
                {
                    writeApi.WritePoint(point, _bucket, _org);

                }
                TimeSpan timeDifference = DateTime.Now - timeStart;
                if (name == "\"PID_Data\"." && item.Name == "y" && timeDifference.TotalSeconds >= 250 && statusZap == true)
                {
                    statusZap = false;
                    ConnectWithAzureFunc.Connect(_zmList);
                    _zmList.Clear();
                }

            }
        }

        public async void CheckChangeInDataBase(bool statusZap,bool statusOnce, float setpoint)
        {
            InfluxDBClient client = (InfluxDBClient)_client;
            string measurement = "change";

            var fluxQuery = $"from(bucket: \"{_bucket}\") |> range(start: -1m) |> filter(fn: (r) => r._measurement == \"{measurement}\")";//var fluxQuery = $"from(bucket: \"{bucket}\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{measurement}\")";
            var queryApi = client.GetQueryApi();

            var fluxTables = await queryApi.QueryAsync(fluxQuery, _org);
            if (fluxTables == null || fluxTables.Count == 0)
            {
                return;
            }
            foreach (var table in fluxTables)
            {
                foreach (var record in table.Records)
                {
                    string name = Convert.ToString(record.GetField());
                    double value = Convert.ToSingle(record.GetValueByKey("_value"));
                    _itemFromInflux.Add(new ItemFromInflux { Name = name, Value = value });

                    if (name == "SetPoint")
                    {
                        float valueD = Convert.ToSingle(value);
                        if (statusOnce == false && valueD != 0)
                        {
                            statusZap = true;
                            statusOnce = true;
                            setpoint = valueD;
                        }
                        else if (statusOnce == true && setpoint != valueD)
                        {
                            //if (valueD < 15.192)
                            //    setpoint = 15.192f;
                            //else
                            setpoint = valueD;

                            statusZap = true;

                            _zmList.Clear();
                        }

                    }

                }
            }
            var deleteApi = client.GetDeleteApi();

            var start = DateTime.UtcNow.AddYears(-100);
            var stop = DateTime.UtcNow;

            // do usuwania danych
            var predicate = $"_measurement=\"{measurement}\"";

            // Wykonanie operacji usuwania
            await deleteApi.Delete(start, stop, predicate, _bucket, _org);//usuwanie danych dla severless nie działa

        }

    }
    
}
