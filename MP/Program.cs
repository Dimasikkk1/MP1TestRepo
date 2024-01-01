using Bogus;
using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MP.Data;
using MP.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBootstrapBlazor();

builder.Services.AddSingleton<WeatherForecastService>();

// 增加 Table 数据服务操作类
builder.Services.AddTableDemoDataService();
builder.Services.AddSingleton<MPContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

#region CreatingContext
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

var mpContext = scope.ServiceProvider.GetRequiredService<MPContext>();

var faker = new Faker<Order>()
    .RuleFor(o => o.Id, f => f.Random.Uuid())
    .RuleFor(o => o.Year, f => 2023)
    .RuleFor(o => o.Quarter, f => f.Random.Number(1, 4))
    .RuleFor(o => o.OrderNumber, f => f.Random.Number())
    .RuleFor(o => o.ReleaseMonth, f => new DateTime(DateTime.Now.Year, f.Random.Number(1, 12), 1))
    .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
    .RuleFor(o => o.Quantity, f => f.Random.Number(1, 100))
    .RuleFor(o => o.Price, f => Math.Round(f.Random.Decimal(1, 100), 2));

var testOrders = faker.Generate(1000);

mpContext.Orders.AddRange(testOrders);
#endregion

var strings = app.Services.GetRequiredService<IStringLocalizer<TableFilter>>().GetAllStrings();

app.Run();
