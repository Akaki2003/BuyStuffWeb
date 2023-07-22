using BuyStuff.GE.API.Infrastructure.Extensions;
using BuyStuff.GE.Persistence;
using BuyStuff.GE.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.OpenApi.Models;
using BuyStuff.GE.API.Infrastructure.Auth.JWT;
using BuyStuff.GE.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventApi",
        Version = "v1",
        Description = "EventApi",
        Contact = new OpenApiContact
        {
            Email = "akaki.ujarashvili@gmail.com",
            Name = "Event Api",
        }
    });
    option.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
        }
                });
    option.CustomSchemaIds(type => type.ToString());
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine($"{AppContext.BaseDirectory}", xmlFile);

    option.IncludeXmlComments(xmlPath);
});
builder.Services.AddTokenAuthentication(builder.Configuration.GetSection(nameof(JWTConfiguration)).GetSection(nameof(JWTConfiguration.Secret)).Value);
builder.Services.AddServices();

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<BuyStuffContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<User>(options => { });
new IdentityBuilder(typeof(User), typeof(IdentityRole), builder.Services)
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<BuyStuffContext>();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));


builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddNewtonsoftJson();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<BuyStuffContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));


builder.Services.AddMvc();

builder.Services.AddScoped<DbContext, BuyStuffContext>();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();


app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Configuration["appSettings:ImagePathBase"], @"images"
    )),
    RequestPath = new PathString("/images")
});
app.MapControllers();



app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
