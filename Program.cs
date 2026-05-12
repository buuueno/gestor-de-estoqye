using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ControleEstoqueApi.Data;
using ControleEstoqueApi.Services;
using ControleEstoqueApi.Middleware;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ✅ JWT obrigatório
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey!);
var securityKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = securityKey,
            ClockSkew = TimeSpan.Zero
        };
    });

// ✅ CORS obrigatório
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ✅ Registrar serviço de autenticação
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers(options => {
    options.Filters.Add(typeof(ValidationFilter));
});
builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger obrigatório
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() { Title = "API - Gestão de Tarefas", Version = "v1" });

    // Diz ao Swagger que existe um esquema de segurança chamado "Bearer"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Cole o token JWT aqui: Bearer {seu_token}"
    });

    // Aplica esse esquema em todos os endpoints automaticamente
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ✅ Middleware de tratamento de erros global
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Controle de Estoque v1"));
}

app.UseHttpsRedirection();
app.UseCors("PermitirTudo");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
