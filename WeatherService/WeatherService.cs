using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherService
{
    public static class WeatherService
    {
        private const string URL = "http://api.openweathermap.org/data/2.5/weather";

        //lat={lat}&lon={lon}
        public static string getWeather(float log, float lat, char unit)
        {
            if (unit != 'C' && unit != 'F')
                return "Invalid temp unit";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string apiKey = Environment.GetEnvironmentVariable("APIKEY");
            string urlParameters = String.Format("?lat={0}&lon={1}&appid=6b34b4b2b9b20b3af3530f12a8a7723e", lat, log);

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var weatherInfo = response.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(weatherInfo);
                float tempKelvin = (float)json["main"]["temp"];
                return convertTemp(tempKelvin, unit).ToString();
            }
            else
            {
                var callError = String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return callError.ToString();
            }
        }
        public static string getWeather(string city, char unit)
        {
            if (unit != 'C' && unit != 'F')
                return "Invalid temp unit";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string apiKey = Environment.GetEnvironmentVariable("APIKEY");
            string urlParameters = String.Format("?q={0}&appid={1}", city, apiKey);

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var weatherInfo = response.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(weatherInfo);
                float tempKelvin = (float)json["main"]["temp"];
                return convertTemp(tempKelvin, unit).ToString();
            }
            else
            {
                var callError = String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return callError.ToString();
            }
        }

        private static float convertTemp(float value, char to)
        {
            if (to == 'C')
                return value - 273.15f;
            else if (to == 'F')
                return (value * 9.0f/5) - 459.67f;

            return 0.0f;
        }

    }
}
