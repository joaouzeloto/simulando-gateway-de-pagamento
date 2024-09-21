using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System.Reflection;



#region Configurando Log
var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
Directory.CreateDirectory(logFolder);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File(new CompactJsonFormatter(),
           Path.Combine(logFolder, ".json"),
            retainedFileCountLimit: 10,
            rollingInterval: RollingInterval.Day)
    .WriteTo.File(Path.Combine(logFolder, ".log"),
            retainedFileCountLimit: 10,
            rollingInterval: RollingInterval.Day)
    .CreateLogger();

#endregion

#region Lendo o appsettings
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

string pathAppsettings = "appsettings.json";

if (env == "Development")
{
    pathAppsettings = "appsettings.Development.json";
}

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(pathAppsettings)
    .Build();

Environment.SetEnvironmentVariable("STRING_CONEXAO", config.GetSection("stringConexao").Value);
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trabalho 1 Bimestre",
        Version = "0.1",
        Description = $@"<h3>Soluções para <b>transações</b></h3>
                        ",
        Contact = new OpenApiContact
        {
            Name = "Site da Unoeste",
            Email = string.Empty,
            Url = new Uri("https://www.unoeste.br"),
        },
    });


    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

#endregion

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

#region IOC 

builder.Services.AddScoped(typeof(TrabalhoPrimeiroBim.services.CartaoService));
builder.Services.AddScoped(typeof(TrabalhoPrimeiroBim.services.PagamentoService));

builder.Services.AddSingleton<TrabalhoPrimeiroBim.BD>(new TrabalhoPrimeiroBim.BD());


#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.RoutePrefix = ""; 
    c.DocumentTitle = "Gerenciamento de Produtos - API V1";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
