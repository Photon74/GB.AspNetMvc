using GB.AspNetMvc.Models;
using GB.AspNetMvc.Models.Repository;
using GB.AspNetMvc.Models.Repository.Interfaces;
using GB.AspNetMvc.Models.Services;
using GB.AspNetMvc.Models.Services.Interfaces;
using MailKit.Net.Smtp;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .WriteTo.Console()
             .CreateBootstrapLogger();
Log.Information("Запуск сервера");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddScoped<IMediator, Mediator>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddSingleton<ICatalogRepository, CatalogInMemory>();
    builder.Services.AddScoped<IMailSenderService, MailSenderByMailKitService>();
    builder.Services.AddScoped<SmtpClient>();

    builder.Host.UseSerilog((_, conf) =>
    {
        conf
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", (LogEventLevel)RollingInterval.Day).MinimumLevel.Debug();
            //.Enrich.FromLogContext();
    });

    builder.Services.AddSingleton<MailSettings>();
    builder.Configuration.AddUserSecrets("1875a7fc-bfba-4c1c-952a-4c53ec409d94");
    var section = builder.Configuration.GetSection("MailSettings");
    builder.Services.Configure<MailSettings>(section);



    var app = builder.Build();

    //Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception e)
{
    if (e.GetType().Name is "StopTheHostException")
    {
        throw;
    }

    Log.Fatal(e, "Сервер упал!");
    throw;
}
finally
{
    Log.Information("Остановка сервера");
    Log.CloseAndFlush();
}
