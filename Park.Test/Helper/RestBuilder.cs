using System.Threading.Tasks;

namespace Park.Test.Helper
{
    public interface IRestBuilder
    {
        IRestBuilder WithRequest(string request);
        IRestBuilder WithHeader(string name, string value);
        IRestBuilder WithQueryParameter(string name, string value);
        IRestBuilder WithUrlSegment(string name, string value);
        IRestBuilder WithBody(object body);
        Task<T?> WithGet<T>();
        Task<T?> WithPost<T>();
        Task<T?> WithPut<T>();
        Task<T?> WithDelete<T>();
        Task<T?> WithPatch<T>();

    }

    public class RestBuilder: IRestBuilder
    {
        private readonly IRestLibrary _restLibrary;

        public RestBuilder(IRestLibrary restLibrary)
        {
            _restLibrary = restLibrary;
        }


        private RestRequest RestRequest { get; set; } = null!;


        public IRestBuilder WithRequest(string request)
        {
            RestRequest = new RestRequest(request);
            return this;
        }
        public IRestBuilder WithHeader(string name, string value)
        {
            this.RestRequest.Add(name, value);
            return this;
        }

        public IRestBuilder WithQueryParameter(string name, string value)
        {
            this.RestRequest.AddQueryParameter(name, value);
            return this;
        }
        public IRestBuilder WithUrlSegment(string name, string value)
        {
            this.RestRequest.AddUrlSegment(name, value);
            return this;
        }
        public IRestBuilder WithBody(object body)
        {
            this.RestRequest.AddJsonBody(body);
            return this;
        }


        public async Task<T?> WithGet<T>()
        {
            return await _restLibrary.RestClient.GetAsync<T>(RestRequest);
        }

        public async Task<T?> WithPost<T>()
        {
            return await _restLibrary.RestClient.PostAsync<T>(RestRequest);
        }


        public async Task<T?> WithPut<T>()
        {
            return await _restLibrary.RestClient.PutAsync<T>(RestRequest);
        }


        public async Task<T?> WithDelete<T>()
        {
            return await _restLibrary.RestClient.DeleteAsync<T>(RestRequest);
        }


        public async Task<T?> WithPatch<T>()
        {
            return await _restLibrary.RestClient.PatchAsync<T>(RestRequest);
        }


    }



}
