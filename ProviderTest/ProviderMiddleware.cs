using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ProviderTest
{
    public class ProviderMiddleware
    {

        private const string ConsumerName = "Consumer1";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;


        //public ProviderMiddleware(RequestDelegate next)
        //{
        //    //_next = next;
        //    //_providerStates = new Dictionary<string, Action>
        //    //{
        //    //    {
        //    //        "There is a profile in the address book with id of 1"
        //    //    }
        //    //};
        //}

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/HotelBookin")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                if (context.Request.Method == HttpMethod.Post.ToString() &&
                    context.Request.Body != null)
                {
                    string jsonRequestBody;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        jsonRequestBody = await reader.ReadToEndAsync();
                    }

                    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                    //A null or empty provider state key must be handled
                    if (providerState != null &&
                        !string.IsNullOrEmpty(providerState.State) &&
                        providerState.Consumer == ConsumerName)
                    {
                        _providerStates[providerState.State].Invoke();
                    }

                    await context.Response.WriteAsync(string.Empty);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }

    }
}
