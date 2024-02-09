using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Madbox
{
    public class RemoteTool
    {
        public static string LATEST_DATA = string.Empty;
        
        public static async Task<T> DownloadAndParse<T>(string url, T defaultValue = default)
        {
            try
            {
                using var client = new HttpClient {Timeout = TimeSpan.FromSeconds(10)};
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError("Error downloading JSON: " + response.ReasonPhrase);
                    return defaultValue;
                }

                LATEST_DATA = await response.Content.ReadAsStringAsync();

                // if caller wants to get the json string
                if (typeof(T) == typeof(string))
                    return (T) Convert.ChangeType(LATEST_DATA, typeof(T));
                
                // Successfully downloaded JSON, now parse it
                return JsonConvert.DeserializeObject<T>(LATEST_DATA);
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during download: " + ex);
                return defaultValue;
            }
        }
    }
}
