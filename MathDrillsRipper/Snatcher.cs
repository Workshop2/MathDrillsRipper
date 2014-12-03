using System.Net;
using CsQuery;
using RestSharp;

namespace MathDrillsRipper
{
    public class Snatcher
    {
        private readonly IConsole _console;
        private readonly RestClient _client;

        public Snatcher(string baseUrl, IConsole console)
        {
            _console = console;
            _client = new RestClient(baseUrl);
        }

        public Page GetPage(string url)
        {
            var request = new RestRequest(url);
            IRestResponse result = _client.Execute(request);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                _console.WriteError("Error while snatching '{0}'", url);
                _console.WriteError(result.ErrorMessage);
                return null;
            }

            CQ document = result.Content;
            return new Page(document);
        }
    }
}