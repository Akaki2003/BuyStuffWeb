using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.Domain.Users.Requests;
using BuyStuff.GE.MVC.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BuyStuff.GE.MVC.ApiServices
{
    public class ItemApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _accessor;
        private readonly string Item = "Item";
        string jwt = null;
        

        public ItemApiService(HttpClient httpClient, IOptions<BaseUriConfiguration> options, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(options.Value.BaseUri);
            _accessor = accessor;
            jwt = _accessor.HttpContext.Request.Cookies["jwt"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public async Task<IEnumerable<ItemModel>> GetAllItems(CancellationToken cancellationToken)
        {
            var result = await _httpClient.GetAsync($"{Item}/GetAllItems", cancellationToken);
            return await result.Content.ReadFromJsonAsync<IList<ItemModel>>();
        }

        public async Task<ItemModel> GetItemById(int itemId, CancellationToken cancellationToken)
        {
            var result = await _httpClient.GetAsync($"{Item}/GetItemById?id={itemId}", cancellationToken);
            return await result.Content.ReadFromJsonAsync<ItemModel>();
        }

        public async Task AddItem(ItemRequestModel item, CancellationToken cancellationToken)
        {
            var jwt = _accessor.HttpContext.Request.Cookies["jwt"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(item.Title), "Title");
                    content.Add(new StringContent(item.Description), "Description");
                    content.Add(new StringContent(item.PhoneNumber), "PhoneNumber");

                    if (item.Images?.Any() == true)
                        foreach (var file in item.Images)
                        {
                            if (file.Length <= 0)
                                continue;

                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                            content.Add(new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                            }, "Images", fileName);
                        }

                    var result = await _httpClient.PostAsync($"{Item}/AddItem", content, cancellationToken);
                }
            
        }

        public async Task UpdateItem(ItemEditModel item, CancellationToken cancellationToken)
        {
            var jwt = _accessor.HttpContext.Request.Cookies["jwt"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(item.Id.ToString()), "Id");
                    content.Add(new StringContent(item.Title), "Title");
                    content.Add(new StringContent(item.Description), "Description");
                    content.Add(new StringContent(item.PhoneNumber), "PhoneNumber");

                    if (item.Images?.Any() == true)
                    {
                        foreach (var file in item.Images)
                        {
                            if (file.Length <= 0)
                                continue;

                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                            content.Add(new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                            }, "Images", fileName);

                        }
                    }
                var result = await _httpClient.PutAsync($"{Item}/UpdateItem", content, cancellationToken);
                }
        }


        public async Task DeleteItem(int itemId, CancellationToken cancellationToken)
        {
            var jwt =_accessor.HttpContext.Request.Cookies["jwt"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var result = await _httpClient.DeleteAsync($"{Item}/DeleteItem/{itemId}", cancellationToken);
        }


       
    }
}
