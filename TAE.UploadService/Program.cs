using Serilog;
using TAE.UploadService.Infrastructure;
using TAE.UploadService.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Logger.Configure(builder.Configuration);
        builder.Host.UseSerilog();

        builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<UploadRepository>();

        bool swaggerEnabled = builder.Configuration.GetValue<bool>("SwaggerEnabled");

        var app = builder.Build();

        if (app.Environment.IsDevelopment() || swaggerEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
