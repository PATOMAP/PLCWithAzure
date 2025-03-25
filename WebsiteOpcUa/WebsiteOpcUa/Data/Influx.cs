using WebsiteOpcUa.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
namespace WebsiteOpcUa.Data
{
    public class Influx
    {
        private string _token;
        private string _bucket;
        private string _org;
        InfluxDBClient _client;
        public List<Item> Items { get; set; }

        public Influx(string token, string bucket, string org, string url)
        {
            Items = new List<Item>();
            _token = token;
            _bucket = bucket;
            _org = org;
            _client = new InfluxDBClient(url, token);

        }

        public async Task ReadDataAsync(string mea, List<string> items)
        {
            Items.Clear();
            string toQuery = "";
            for (int i = 0; i < items.Count; i++)
            {
                if (i != items.Count - 1)
                {
                    toQuery = toQuery + $@"r._field == ""{items[i]}"" or ";
                }
                else
                {
                    toQuery = toQuery + $@"r._field == ""{items[i]}""";

                }

            }
            //    |> range(start: -1m)

            var fluxQuery = $@"
            from(bucket: ""{_bucket}"")
            |> range(start: -1m)
            |> filter(fn: (r) => r._measurement == ""{mea}"")
            |> filter(fn: (r) => {toQuery})
            |> yield(name: ""result"") ";
            var queryApi = _client.GetQueryApi();
            List<FluxTable> fluxTables = await queryApi.QueryAsync(fluxQuery, _org);

                foreach (var table in fluxTables)
                {
                    foreach (var record in table.Records)
                    {
                        double value = Convert.ToDouble(record.GetValue());
                        string field = record.GetField();
                        foreach (var item in items)
                        {
                            if (item == field)
                            {
                                if (Items.Any(x => x.Name == field))
                                {
                                    Item itemDB = Items.Find(x => x.Name == field);
                                    itemDB.Value = Math.Round(value,2);
                                }
                                else
                                    Items.Add(new Item { Name = item, Value = Math.Round(value, 2) });//value

                                break;
                            }
                        }

                    }
                }

            try
            {
            }
            catch (Exception ex)
            {

            }
        }

        public void writeData(Item db)
        {
            string name = db.Name;

            var point = PointData
           .Measurement("change")
           .Tag("host", "host1")
           .Field(name, db.Value)
           .Timestamp(DateTime.Now, WritePrecision.Ns);
            Console.WriteLine(DateTime.Now);

            using (var writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(point, _bucket, _org);
            }
        }
    }
}
