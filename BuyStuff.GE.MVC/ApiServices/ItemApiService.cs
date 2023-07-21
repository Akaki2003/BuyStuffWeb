using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.MVC.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace BuyStuff.GE.MVC.ApiServices
{
    public class ItemApiService
    {
        private readonly HttpClient _httpClient;

        public ItemApiService(HttpClient httpClient, IOptions<BaseUriConfiguration> options)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(options.Value.BaseUri);
        }

        public async Task<IEnumerable<ItemModel>> GetAllItems(CancellationToken cancellationToken)
        {
            var result = await _httpClient.GetAsync("GetAllItems", cancellationToken);
            return await result.Content.ReadFromJsonAsync<IList<ItemModel>>();
        }

        public async Task<ItemModel> GetItemById(int itemId, CancellationToken cancellationToken)
        {
            var result = await _httpClient.GetAsync($"GetItemById?id={itemId}", cancellationToken);
            return await result.Content.ReadFromJsonAsync<ItemModel>();
        }

        public async Task AddItem(ItemRequestModel item, CancellationToken cancellationToken)
        {
            using (var client = _httpClient)
            {

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

                    var response = await client.PostAsync("AddItem", content);
                }
            }
        }

        public async Task UpdateItem(ItemEditModel item, CancellationToken cancellationToken)
        {
            using (var client = _httpClient)
            {

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

                    var response = await client.PutAsync("UpdateItem", content);
                }
            }
        }

        public async Task DeleteItem(int itemId, CancellationToken cancellationToken)
        {
            await _httpClient.DeleteAsync($"DeleteItem/{itemId}", cancellationToken);
        }

       
    }
}
