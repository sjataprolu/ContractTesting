using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;
using static System.Net.WebRequestMethods;

namespace HotelBookingProviderTests
{
    public class UnitTest1
    {

        private ITestOutputHelper _outputHelper { get; }

        [Fact]
        public void Test1()
        {

       
            var config = new PactVerifierConfig
            {



                // NOTE: We default to using a ConsoleOutput,
                // however xUnit 2 does not capture the console output,
                // so a custom outputter is required.
                Outputters = new List<IOutput>
                                {
                                    new XunitOutput(_outputHelper)
                                },



                // Output verbose verification logs to the test output
                LogLevel = PactNet.PactLogLevel.Debug
            };

            IPactVerifier pactVerifier = new PactVerifier();
            var pactFile = new FileInfo(Path.Join("..", "..", "..", "..", "..", "pacts", "Consumer-Provider.json"));
            pactVerifier.ServiceProvider("Provider", new Uri("https://localhost:7117/"))
            .WithFileSource(pactFile)
            .WithProviderStateUrl(new Uri($"https://localhost:7117/api/HotelBookin?id=1"))
            .Verify();
        }

    }
}