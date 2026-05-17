using Inventario_Back.interfaces;
using Inventario_Back.services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddScoped<IProductoService, ProductoService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();