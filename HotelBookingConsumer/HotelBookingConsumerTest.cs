using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PactNet;
using PactNet.Matchers;
using Xunit;

namespace HotelBookingConsumer
{

    public class HotelBookingConsumerTest
    {
        private IPactBuilderV3 pact;


        public HotelBookingConsumerTest()
        {
                var config = new PactConfig
                {
                    PactDir = Path.Join("..", "..", "..", "pacts"),
                    // Outputters = new[] { new XunitOutput(output) },
                    LogLevel = PactLogLevel.Debug
                };

        pact = Pact.V3("Consumer", "Provider", config).WithHttpInteractions();

        }

        [Fact]
        public async Task OnMessageAsync() 
        {
            pact
                   .UponReceiving("A GET request to retrieve the Booking")
                   .Given("an booking with ID {id} exists")
                    .WithRequest(HttpMethod.Get, "/api/HotelBookin/1")
                    .WithHeader("Accept", "application/json")
                    .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithJsonBody(new
                    {
                        Id = 1,
                        RoomNumber = "100",
                        ClientName = "Tester",
                    });

            await pact.VerifyAsync(async ctx =>
            {
                // var result = await ConsumerAPIClient.GetProfileById("1",ctx.MockServerUri.ToString());
                var consumer = new BookingClient(ctx.MockServerUri.ToString());
                var result = consumer.GetProfileById("1");
                Assert.Equal(1, result.Id);
                Assert.Equal("100", result.RoomNumber);
                Assert.Equal("Tester", result.ClientName);
            });


        }

    }
}
