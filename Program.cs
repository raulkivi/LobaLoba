using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Logging);

var app = builder.Build();

// Add additional logger after app.Build()
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddProvider(new ChatLoggerProvider(LogLevel.Debug, app.Services.GetRequiredService<IHubContext<LogHub>>()));

// Configure the HTTP request pipeline.
ConfigureRequestPipeline(app);

app.Run();

static void ConfigureServices(IServiceCollection services, ILoggingBuilder logging)
{
    services.AddSignalR();
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });

    logging.ClearProviders();
    logging.AddConsole();


}

static void ConfigureRequestPipeline(WebApplication app)
{
    app.UseHttpsRedirection();

    app.UseStaticFiles(); // Serve static files

    app.UseRouting();

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<LogHub>("/logHub");

    // Configure the SPA middleware runn in root path
    app.UseWhen(context =>
    !context.Request.Path.StartsWithSegments("/api") &&
    !context.Request.Path.StartsWithSegments("/swagger") &&
    !context.Request.Path.StartsWithSegments("/logHub")
    , appBuilder =>
    {
        appBuilder.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if (app.Environment.IsDevelopment())
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    });

    // Fallback to serve the React app for any non-API routes
    //app.MapFallbackToFile("index.html");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}