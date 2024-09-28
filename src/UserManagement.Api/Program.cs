using Serilog;
using UserManagement.Api.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
    });

    builder.Services.ConfigureServices(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //app.ApplyMigrations();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseUsersSecurityStampChecker();

    app.MapControllers();

    app.UseCors("BlazorApp");

    app.Run();
}

