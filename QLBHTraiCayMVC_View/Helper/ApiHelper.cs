using QLBHTraiCayMVC_View.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace QLBHTraiCayMVC_View.Helper
{
    public class ApiHelper<T>
    {
        //static string apiURL = "http://localhost/QLBHWebAPI/api/";
        static string apiURL = ConfigurationManager.AppSettings["apiURL"];
        public static async Task<dynamic> RunGetAsync(string url)
        {
            dynamic result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var err = await response.Content.ReadAsAsync<BadRequestGet>();
                    throw new Exception(err.Message);
                }
                else
                {
                    throw new Exception("Lỗi kết nối với service");
                }
            }
            return result;
        }

        public static dynamic RunGet(string url)
        {
            dynamic result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<T>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var err = response.Content.ReadAsAsync<BadRequestGet>().Result;
                    throw new Exception(err.Message);
                }
                else
                {
                    throw new Exception("Lỗi kết nối với service");
                }
            }
            return result;
        }

        public static async Task<dynamic> RunPostAsync(string url, object input = null)
        {
            dynamic result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(url, input);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var err = await response.Content.ReadAsAsync<BadRequestPost>();
                    string errMsg = null;
                    if (err.ModelState == null) //Dùng cho Xóa
                    {
                        errMsg = err.Message;
                    }
                    else // khác xóa
                    {
                        foreach (KeyValuePair<string, string[]> item in err.ModelState)
                        {
                            errMsg += $"{string.Join(",", item.Value)}";
                        }
                    }
                    throw new Exception(errMsg);
                }
                else
                {
                    throw new Exception("Lỗi kết nối với service");
                }
            }
            return result;
        }
    }
}