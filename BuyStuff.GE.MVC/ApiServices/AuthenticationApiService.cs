using BuyStuff.GE.Domain.Users.Requests;
using BuyStuff.GE.MVC.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace BuyStuff.GE.MVC.ApiServices
{
    public class AuthenticationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string Auth = "Authenticate";

        public AuthenticationApiService(HttpClient httpClient, IOptions<BaseUriConfiguration> options)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(options.Value.BaseUri);
        }

        public async Task<string> Login(LoginModel model, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{Auth}/login", data, cancellationToken);

            return await result.Content.ReadAsStringAsync(cancellationToken); //jwt
        }


        public async Task<string> Register(RegisterModel model, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync($"{Auth}/register", data, cancellationToken);

            return await result.Content.ReadAsStringAsync(cancellationToken); //jwt
        }
    }
}
