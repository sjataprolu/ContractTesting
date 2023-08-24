using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using System.Net;

namespace HotelBookingConsumer
{
    public class BookingClient
    {
        //public long Id { get; set; 




        public BookingClient(string baseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri ?? "https://localhost:5001") };
        }
        private readonly HttpClient _client;
        public class Profile
        {
            public int Id { get; set; }
            
            public string? RoomNumber { get; set; }  
            public string? ClientName { get; set; }
        }

        public Profile? GetProfileById(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/HotelBookin/{id}");
            request.Headers.Add("Accept", "application/json");

            var response = _client.SendAsync(request);

            var content = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            var reasonPhrase = response.Result
                .ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<Profile>(content)
                    : null;
            }

            throw new Exception(reasonPhrase);
        }



    }
}
