using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Configuration;



namespace AzureFunctionOpcUa
{
    public class tempItemInflux
    {
        public TimeSpan Time { get; set; }
        public float ValueF { get; set; }

        public float Setpoint { get; set; }
    }
    public class itemToWrite
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }


    public class InfluxConnect
    {

        string token = ConfigurationManager.ConnectionStrings["token"].ConnectionString;
        InfluxDBClient _client;
        string bucket = "dataopcua";
        string org = "opcua";

        public InfluxConnect()
        {
            _client = new InfluxDBClient("http://localhost:8086", token);
        }

        public async void writePoint(double wart, string name)
        {
            var point = PointData
            .Measurement("mem3")
            .Tag("host", "host1")
            .Field(name, wart)
            .Timestamp(DateTime.Now, WritePrecision.Ns);
            try
            {
                // Zapis asynchroniczny danych do InfluxDB
                var writeApiAsync = _client.GetWriteApiAsync();
                await writeApiAsync.WritePointAsync(point, bucket, org);
            }
            catch (Exception ex)
            {


            }
        }
    }

    public static class Average
    {


        [FunctionName("Average")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            InfluxConnect _connect = new InfluxConnect();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<tempItemInflux> data = JsonConvert.DeserializeObject<List<tempItemInflux>>(requestBody);
            List<itemToWrite> total = new List<itemToWrite>();

            if (data[0].ValueF > data[data.Count - 1].ValueF)
                total = downFunction(data);

            else
                total = upFunction(data);


            for (int i = 0; i < total.Count; i++)
            {

                _connect.writePoint(total[i].Value, total[i].Name);
                data.Clear();
            }

            return new OkObjectResult("");
        }

        public static List<itemToWrite> downFunction(List<tempItemInflux> dataPoints)
        {
            List<itemToWrite> result = new List<itemToWrite>();
            double ISE = 0;
            double dt = Math.Round((dataPoints[1].Time - dataPoints[0].Time).TotalSeconds, 2);

            foreach (var point in dataPoints)
            {
                if (point.Setpoint < 15.192f)
                {
                    point.Setpoint = 15.192f;
                }
                ISE += Math.Pow((point.Setpoint - point.ValueF), 2) * dt;
            }
            double minValue = dataPoints.Min(point => point.ValueF);
            double przerugolowanie = Math.Abs((((minValue - dataPoints[0].ValueF) - (dataPoints[dataPoints.Count - 1].Setpoint - dataPoints[0].ValueF)) / (dataPoints[dataPoints.Count - 1].Setpoint - dataPoints[0].ValueF)) * 100);

            float subEl = 0;
            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (i == 0)
                    subEl = dataPoints[dataPoints.Count - 1].Setpoint;

                dataPoints[i].ValueF = dataPoints[i].ValueF - subEl;
                dataPoints[i].Setpoint = dataPoints[i].Setpoint - subEl;
            }

            double halfValue = dataPoints[0].ValueF / 2;
            tempItemInflux closestPoint = dataPoints
            .OrderBy(point => Math.Abs(point.ValueF - halfValue))
            .First();
            TimeSpan timeDelay = closestPoint.Time - dataPoints[0].Time;

            double tenProcentValueF = dataPoints[0].ValueF * 0.9;
            double ninetyProcentValueF = dataPoints[0].ValueF * 0.1;

            tempItemInflux closestPointTen = dataPoints
           .OrderBy(point => Math.Abs(point.ValueF - tenProcentValueF))
           .First();

            tempItemInflux closestPointNinety = dataPoints
            .OrderBy(point => Math.Abs(point.ValueF - ninetyProcentValueF))
            .First();
            TimeSpan czasNarastania = closestPointNinety.Time - closestPointTen.Time;


            double steadyStateValueF = dataPoints[dataPoints.Count - 1].Setpoint;


            double tolerance = 0.5;


            double lowerBound = steadyStateValueF - tolerance;
            double upperBound = steadyStateValueF + tolerance;


            TimeSpan settlingTime = TimeSpan.Zero;


            for (int i = 0; i < dataPoints.Count; i++)
            {
                bool allInToleranceAfter = dataPoints
                    .Skip(i)
                    .All(point => point.ValueF >= lowerBound && point.ValueF <= upperBound);

                if (allInToleranceAfter)
                {
                    settlingTime = dataPoints[i].Time;
                    settlingTime = settlingTime - dataPoints[0].Time;
                    break;
                }
            }






            result.Add(new itemToWrite { Name = "przerugolowanie[%]", Value = przerugolowanie });
            result.Add(new itemToWrite { Name = "ISE", Value = ISE });
            result.Add(new itemToWrite { Name = "czasNarastania", Value = Math.Round(czasNarastania.TotalSeconds, 2) });
            result.Add(new itemToWrite { Name = "timeDelay", Value = Math.Round(timeDelay.TotalSeconds, 2) });
            result.Add(new itemToWrite { Name = "czasRegulacji", Value = Math.Round(settlingTime.TotalSeconds, 2) });


            return result;

        }
        public static List<itemToWrite> upFunction(List<tempItemInflux> dataPoints)
        {
            List<itemToWrite> result = new List<itemToWrite>();
            float subEl = 0;
            double ISE = 0;
            double dt = Math.Round((dataPoints[1].Time - dataPoints[0].Time).TotalSeconds, 2);


            foreach (var point in dataPoints)
            {
                if (point.Setpoint < 15.192f)
                {
                    point.Setpoint = 15.192f;
                }
                ISE += Math.Pow((point.Setpoint - point.ValueF), 2) * dt;
            }
            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (i == 0)
                {
                    subEl = dataPoints[0].ValueF;
                    dataPoints[0].ValueF = 0;
                    dataPoints[0].Setpoint = dataPoints[0].Setpoint - subEl;
                }

                else
                {
                    dataPoints[i].ValueF = dataPoints[i].ValueF - subEl;
                    dataPoints[i].Setpoint = dataPoints[i].Setpoint - subEl;
                }

            }

            double maxValue = dataPoints.Max(point => point.ValueF);
            double przerugolowanie = ((maxValue / dataPoints[dataPoints.Count - 1].Setpoint) - 1) * 100;

            double halfValue = dataPoints[dataPoints.Count - 1].Setpoint / 2;

            tempItemInflux closestPoint = dataPoints
            .OrderBy(point => Math.Abs(point.ValueF - halfValue))
            .First();

            TimeSpan timeDelay = closestPoint.Time - dataPoints[0].Time;

            double tenProcentValue = dataPoints[dataPoints.Count - 1].Setpoint * 0.1;
            double ninthyProcentValue = dataPoints[dataPoints.Count - 1].Setpoint * 0.9;

            tempItemInflux closestPointTen = dataPoints
           .OrderBy(point => Math.Abs(point.ValueF - tenProcentValue))
            .First();

            tempItemInflux closestPointNinety = dataPoints
            .OrderBy(point => Math.Abs(point.ValueF - ninthyProcentValue))
            .First();
            TimeSpan czasNarastania = closestPointNinety.Time - closestPointTen.Time;

            double steadyStateValue = dataPoints[dataPoints.Count - 1].Setpoint;


            double tolerance = 0.5;


            double lowerBound = steadyStateValue - tolerance;
            double upperBound = steadyStateValue + tolerance;


            TimeSpan settlingTime = TimeSpan.Zero;


            for (int i = 0; i < dataPoints.Count; i++)
            {
                bool allInToleranceAfter = dataPoints
                    .Skip(i)
                    .All(point => point.ValueF >= lowerBound && point.ValueF <= upperBound);

                if (allInToleranceAfter)
                {
                    settlingTime = dataPoints[i].Time;
                    settlingTime = settlingTime - dataPoints[0].Time;
                    break;
                }
            }



            result.Add(new itemToWrite { Name = "przerugolowanie[%]", Value = przerugolowanie });
            result.Add(new itemToWrite { Name = "ISE", Value = ISE });
            result.Add(new itemToWrite { Name = "czasNarastania", Value = Math.Round(czasNarastania.TotalSeconds, 2) });
            result.Add(new itemToWrite { Name = "timeDelay", Value = Math.Round(timeDelay.TotalSeconds, 2) });
            result.Add(new itemToWrite { Name = "czasRegulacji", Value = Math.Round(settlingTime.TotalSeconds, 2) });


            return result;

        }


    }
}
