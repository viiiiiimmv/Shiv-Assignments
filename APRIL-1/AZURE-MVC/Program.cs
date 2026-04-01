using AZURE_MVC.Services;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);
var blobConnection = builder.Configuration.GetValue<string>("BlobConnection");

if (string.IsNullOrWhiteSpace(blobConnection) || blobConnection.Contains("REPLACE_WITH", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException(
        "Set 'BlobConnection' in configuration or the 'BlobConnection' environment variable before running AZURE-MVC.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(_ => new BlobServiceClient(blobConnection));
builder.Services.AddSingleton<IContainerService, ContainerService>();
builder.Services.AddSingleton<IBlobService, BlobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
