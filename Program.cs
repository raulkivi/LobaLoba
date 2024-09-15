using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigureRequestPipeline(app);

app.Run();

static void ConfigureServices(IServiceCollection services)
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
}

static void ConfigureRequestPipeline(WebApplication app)
{
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseStaticFiles(); // Serve static files

    app.UseRouting();

    app.UseCors("AllowAll");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<LogHub>("/logHub"); // Add this line to map the LogHub endpoint
    });

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