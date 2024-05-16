using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Security;
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
var Jwt = builder.Configuration.GetSection("JWT").Get<JWT>();
//Agregar Autorizacion para el jwt
builder.Services.AddAuthorization();

if (Jwt != null)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    x =>
    {
        x.TokenValidationParameters = new()
        {
            ValidIssuer = Jwt.Issuer,
            ValidAudience = Jwt.Audiance,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key ?? "")),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true
        };
    });
}
#endregion
#endregion

#region Transient`s
builder.Services.AddSingleton<JWTHelper>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ActividadesRepository>();
builder.Services.AddTransient<DepartamentosRepository>();
#endregion
#endregion
#region Base de datos
var Db = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ItesrcneActividadesContext>
(
    x => x.UseMySql(Db, ServerVersion.AutoDetect(Db))
);
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion