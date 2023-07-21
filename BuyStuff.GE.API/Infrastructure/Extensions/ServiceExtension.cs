using BuyStuff.GE.Application.Images.Repositories;
using BuyStuff.GE.Application;
using BuyStuff.GE.Application.Items;
using BuyStuff.GE.Infrastructure.UOW;
using BuyStuff.GE.Application.Items.Repositories;
using BuyStuff.GE.Infrastructure.Items;
using BuyStuff.GE.Infrastructure.Images;

namespace BuyStuff.GE.API.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IItemService, ItemService>();


            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

    }
}
