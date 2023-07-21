using BuyStuff.GE.Application.Items.Repositories;
using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.Application.Items.Responses;
using BuyStuff.GE.Domain.Images;
using BuyStuff.GE.Domain.Items;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuyStuff.GE.Application.Items
{
    public class ItemService : IItemService
    {
        IItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _configuration;

        public ItemService(IUnitOfWork unitOfWork,IItemRepository repository,IConfiguration configuration)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<int> CreateItem(ItemRequestModel itemRequest, CancellationToken cancellationToken)
        {
            if (itemRequest != null)
            {
                string directoryPath = Path.Combine(_configuration["appSettings:ImagePathBase"], "images");
                var images = new List<Image>();
                if(itemRequest.Images?.Any() == true)
                {
                    foreach (var item in itemRequest.Images)
                    {
                        if (item.FileName == null || item.FileName.Length == 0)
                        {
                            throw new FileLoadException("File not selected");
                        }
                        var path = Path.Combine(directoryPath, item.FileName);


                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                            stream.Close();
                        }


                        var imageModel = new Image
                        {
                            ImgName = item.FileName,
                            ImagePath = path,

                        };
                        var img = await _unitOfWork.Image.AddImage(imageModel, cancellationToken);
                        images.Add(img);
                    }
                }
                
                var itemToCreate = itemRequest.Adapt<Item>();
                itemToCreate.Images = images.Count>0?images:itemToCreate.Images;
                int id = await _unitOfWork.Item.Create(itemToCreate, cancellationToken);
                return id;
            }
            throw new Exception("Item is empty");
        }

        public async Task<List<ItemResponseModel>> GetAllItems(CancellationToken cancellationToken)
        {
            return(await _repository.GetAllItems(cancellationToken)).Adapt<List<ItemResponseModel>>();
        }

        public async Task<ItemResponseModel> GetItemById(int id, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Item.GetItemById(id, cancellationToken);
            return item.Adapt<ItemResponseModel>();
        }

        public async Task DeleteItem(int id, CancellationToken cancellationToken)
        {
            var itemToDelete = await _unitOfWork.Item.GetItemById(id, cancellationToken) ?? throw new Exception("item with provided id was not found");
            await _unitOfWork.Item.Delete(itemToDelete.Id, cancellationToken);
            await _unitOfWork.Save(cancellationToken);
        }

        public async Task UpdateItem( ItemRequestPutModel itemRequest, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _unitOfWork.Item.GetItemById(itemRequest.Id, cancellationToken)?? throw new Exception("Item is empty");
            itemToUpdate.PhoneNumber = itemRequest.PhoneNumber;
            itemToUpdate.Title = itemRequest.Title;
            itemToUpdate.Description = itemRequest.Description;

            if (itemRequest != null)
            {
                string directoryPath = Path.Combine(_configuration["appSettings:ImagePathBase"], "images");
                var images = new List<Image>();
                if(itemRequest.Images?.Any() == true)
                {
                    foreach (var item in itemRequest.Images)
                    {
                        if (item.FileName == null || item.FileName.Length == 0)
                        {
                            throw new FileLoadException("File not selected");
                        }
                        var path = Path.Combine(directoryPath, item.FileName);


                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                            stream.Close();
                        }


                        var imageModel = new Image
                        {
                            ImgName = item.FileName,
                            ImagePath = path,

                        };
                        var img = await _unitOfWork.Image.AddImage(imageModel, cancellationToken);
                        images.Add(img);
                    }
                    itemToUpdate.Images.Clear();
                    itemToUpdate.Images = images;
                }
               
                await _unitOfWork.Item.Update(itemToUpdate, cancellationToken);
                await _unitOfWork.Save(cancellationToken);
                return;
            }
            throw new Exception("Item is empty");
        }

    }
}
