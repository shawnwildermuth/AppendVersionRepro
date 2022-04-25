using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

// Build the different providers you need
var webRootProvider = new PhysicalFileProvider(builder.Environment.WebRootPath);
var newPathProvider = new PhysicalFileProvider(
  Path.Combine(builder.Environment.ContentRootPath, @"Other"));

// Create the Composite Provider
var compositeProvider = new CompositeFileProvider(webRootProvider,
                                                  newPathProvider);

// Replace the default provider with the new one
app.Environment.WebRootFileProvider = compositeProvider;

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
  // Add the other folder, using the Content Root as the base
  FileProvider = new PhysicalFileProvider(
    Path.Combine(builder.Environment.ContentRootPath, "OtherFolder"))
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
