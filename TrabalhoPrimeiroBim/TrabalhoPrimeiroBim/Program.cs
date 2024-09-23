using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;



#region Configurando Log
var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
Directory.CreateDirectory(logFolder);

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .WriteTo.File(new CompactJsonFormatter(),
          Path.Combine(logFolder, "log.json"),  
           retainedFileCountLimit: 1,            
           rollingInterval: RollingInterval.Day) 
   .WriteTo.File(Path.Combine(logFolder, "log.txt"), 
           retainedFileCountLimit: 1,               
           rollingInterval: RollingInterval.Day)    
   .WriteTo.MySQL("Server=129.148.59.75;Database=aluno8;Uid=aluno8;Pwd=12345678Xx$;",
                          "Logs")
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
        Description = $@"<h3>Gateway de <b>pagamento</b></h3>
                        ",
        Contact = new OpenApiContact
        {
            Name = "Github: projeto",
            Email = string.Empty,
            Url = new Uri("https://github.com/joaouzeloto/simulando-gateway-de-pagamento"),
        },
    });


    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "<b>Informe dentro do campo a palavra \"Bearer\" segundo por espaço e o APIKEY. Exemplo: Bearer SDJKF83248923</b>",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {securityScheme, new string[] { }}
        });
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
builder.Services.AddScoped(typeof(TrabalhoPrimeiroBim.domain.Pagamento));

builder.Services.AddScoped(typeof(TrabalhoPrimeiroBim.services.PagamentoService));

builder.Services.AddSingleton<TrabalhoPrimeiroBim.BD>(new TrabalhoPrimeiroBim.BD());


#endregion

#region JWT
var userClaims = new List<Claim>();
userClaims.Add(new Claim(ClaimTypes.Name, "Usuario")); //Claim padrão
userClaims.Add(new Claim(ClaimTypes.Role, "Administrador"));
userClaims.Add(new Claim("data", DateTime.Now.ToString()));

var identidade = new ClaimsIdentity(userClaims);

var handler = new JwtSecurityTokenHandler();

string minhaKey = "minha-chave-secreta-minha-chave-secreta-super"; // >= 32 caracteres
SecurityKey key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(minhaKey));
SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
{
    Audience = "Usuários da API", //Quem vai usar o token, público-alvo
    Issuer = "Joao Uzeloto", //Emisssor
    NotBefore = DateTime.Now, //Data de início
    Expires = DateTime.Now.AddYears(1), //Data fim 
    Subject = identidade, // credenciais de assinatura + Claims
    SigningCredentials = signingCredentials //a chave para criptografar os dados
};

var dadosToken = handler.CreateToken(tokenDescriptor);
string jwt = handler.WriteToken(dadosToken);
Console.Write(jwt);
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
