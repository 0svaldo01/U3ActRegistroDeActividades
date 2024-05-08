using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using U3ActRegistroDeActividadesApi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);
#region Servicios
builder.Services.AddControllers();
#region Agregar Swagger con JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RegistroActividadesApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token recibido por el login",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion
#endregion
#region Base de datos
var Db = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddMySql<ItesrcneActividadesContext>(Db, ServerVersion.AutoDetect(Db));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
