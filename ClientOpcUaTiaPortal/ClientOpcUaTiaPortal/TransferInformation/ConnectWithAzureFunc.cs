using ClientOpcUaTiaPortal.item;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientOpcUaTiaPortal.TransferInformation
{
    public class ConnectWithAzureFunc
    {
        public static async void Connect(List<tempItemInflux> list)//new class
        {

            string functionUrl = "http://localhost:7281/api/Average";


            string jsonData = JsonConvert.SerializeObject(list);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Tworzenie treści żądania HTTP POST
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Wysłanie żądania do Azure Function
                    HttpResponseMessage response = await client.PostAsync(functionUrl, content);

                    // Sprawdzenie odpowiedzi
                    if (response.IsSuccessStatusCode)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not access to AzureFunction");
            }



        }
    }
}
