using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
#region Servicios                                                                        
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddJsonOptions(x => x.JsonSerializerOptions.MaxDepth = 6);
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
#region
builder.Services.AddTransient<DepartamentosRepository>();

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
