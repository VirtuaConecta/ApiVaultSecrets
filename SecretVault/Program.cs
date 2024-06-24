using SecretVault.Application.Interfaces;
using SecretVault.Domain.Interfaces;
using SecretVault.Infrastucture.Configuration;
using SecretVault.Infrastucture.Data.Inicialization;
using SecretVault.Infrastucture.Services;
using SecretVault.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//JWT key
Settings.Secret = builder.Configuration["JwtSettings:Secret"];

//Store connection string
Settings.ConnectionString = builder.Configuration["ConnectionStrings:SecretVaultContext"] ?? String.Empty ;

string primaryEncryptionKey = Environment.GetEnvironmentVariable("PRIMARY_ENCRYPTION_KEY") ?? String.Empty ;


if (string.IsNullOrEmpty(primaryEncryptionKey))
{
    throw new InvalidOperationException("PRIMARY_ENCRYPTION_KEY is not defined");
}

var encryptionService = new EncryptionService(primaryEncryptionKey);

// this Configures Services
builder.Services.AddApplicationServices(builder.Configuration, encryptionService);



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// User Inicializer to create Admin credential in User table.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userRepository = services.GetRequiredService<IUserRepository>();
    var encryption  = services.GetRequiredService<IEncryptionService>();

    await  DataInitializer.InitializeAsync(userRepository, encryption );
}



app.MapSecretEndpoints(encryptionService);


app.Run();

