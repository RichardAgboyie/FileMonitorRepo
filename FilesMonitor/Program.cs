using System.Reflection;
using BusinessLogic;
using BusinessLogic.Interfaces;
using Entities;
using Entities.Entities;
using FilesMonitor.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddCommandLine(args)
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = config["ConnectionStrings:DBConnectionString"];

builder.Services.AddDbContext<SftpDbContext>(x => x.UseSqlServer(connectionString));

var pathSetting = builder.Configuration.GetSection("PathSetting");
builder.Services.Configure<PathSetting>(pathSetting);

//add swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Files Monitor",
        new OpenApiInfo
        {
            Title = "Files Monitor Test",
            Version = "v1",
            Description = "Test File Upload using SFTP",
            Contact = new OpenApiContact
            {
                Email = "",
                Name = "Files Upload Tester",
            }
        });
    //get xml comments
    var xmlCommentsFiles = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFiles);
    options.IncludeXmlComments(xmlCommentsFullPath);
});

//inject business and repository
//builder.Services.AddScoped<ISftpRepository, SftpRepository>();

//inject hangfire
//builder.Services.AddHangfire(x=>x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//inject scheduler
builder.Services.AddScoped<IJobScheduler, JobScheduler>();

//inject business layer
builder.Services.AddTransient<ISftpBusiness, SftpBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("/Assets/custom-ui.css");
        options.SwaggerEndpoint("/swagger/FilesMonitorAPITester/swagger.json", "Files Monitor Tester");
        options.DefaultModelExpandDepth(2);
        options.DefaultModelRendering(ModelRendering.Model);
        options.DocExpansion(DocExpansion.None);
        options.EnableDeepLinking();
        options.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hang Fire Dashboard"
});
app.MapHangfireDashboard();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();