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

        public static string getWeather(string city, char unit)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string urlParameters = String.Format("?q={0}&appid=6b34b4b2b9b20b3af3530f12a8a7723e", city);

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
