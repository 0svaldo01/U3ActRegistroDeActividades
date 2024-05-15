using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Repositories;
var builder = WebApplication.CreateBuilder(args);
#region Servicios
builder.Services.AddControllers()
    //Hace que no haya problemas con los loops en la base de datos
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
#region JWT
//Agregar Autorizacion para el jwt
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
(
    jwt => jwt.TokenValidationParameters = new()
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audiance"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "")),
        ValidateAudience = true,
        ValidateIssuer = true
    }
);
#endregion

#endregion
#region Transient`s
//Esta linea de codigo evita utilizar el AddTransient varias veces
//pero necesita la implementacion de la interface extraida del repositorio generico
builder.Services.AddTransient(typeof(IGenericRepository<>));
#endregion
#endregion
#region Base de datos
var Db = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddMySql<ItesrcneActividadesContext>(Db, ServerVersion.AutoDetect(Db));
#endregion
var app = builder.Build();
#region Implementacion Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion
#region Configuracion
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion