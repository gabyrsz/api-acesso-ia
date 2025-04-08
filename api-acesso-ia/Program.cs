using api_acesso_ia;
using api_acesso_ia.Repositories;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services;
using api_acesso_ia.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:8080") // ou a porta real do seu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(op =>
    op.UseMySql(connectionString, ServerVersion.Parse("5.7.0"))
);

// Injeção de dependências
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

builder.Services.AddScoped<IAcessoService, AcessoService>();
builder.Services.AddScoped<IAcessoRepository, AcessoRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline da aplicação
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  ATENÇÃO: UseCors deve vir ANTES do Authorization
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.Run();
