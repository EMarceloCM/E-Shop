using EShopWeb.Services;
using EShopWeb.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ProductApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:ProductApi"]!);
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAuthentication(op =>
{
    op.DefaultScheme = "Cookies";
    op.DefaultChallengeScheme = "iodc";
}).AddCookie("Cookies", c=> c.ExpireTimeSpan = TimeSpan.FromMinutes(10)).AddOpenIdConnect("iodc", op =>
{
    op.Authority = builder.Configuration["ServiceUri:IdentityServer"];
    op.GetClaimsFromUserInfoEndpoint = true;
    op.ClientId = "eshop";
    op.ClientSecret = builder.Configuration["Client:Secret"];
    op.ResponseType = "code";
    op.ClaimActions.MapJsonKey("role", "role", "role");
    op.ClaimActions.MapJsonKey("sub", "sub", "sub");
    op.TokenValidationParameters.NameClaimType = "name";
    op.TokenValidationParameters.RoleClaimType = "role";
    op.Scope.Add("eshop");
    op.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
