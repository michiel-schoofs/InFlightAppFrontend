using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;

namespace InFlightApp.Services
{
    public class ApiConnection{
        public static string Token { get; set; }
        public static string URL { get => "https://localhost:44355/api"; }
        public static string ChatURL { get => "http://localhost:51178/chatHub"; }
        public static HttpClient Client { get{
                //Doesn't check the ssl certificate on the API so it won't complain :)
                HttpClientHandler handler = new HttpClientHandler{
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                HttpClient client = new HttpClient(handler);

                if (Token != null) {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                }

                return client;
            }
        }
    }
}
