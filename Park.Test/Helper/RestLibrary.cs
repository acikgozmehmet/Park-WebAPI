using System.Net.Http;

namespace Park.Test.Helper
{
    public class RestLibrary: IRestLibrary
    {
        public RestLibrary()
        {
            RestClient = new HttpClient();
        }
        public HttpClient RestClient { get; }
    }

    public interface IRestLibrary
    {
    }
}
