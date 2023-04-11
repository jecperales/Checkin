using api.checkin.data;
using api.checkin.data.Repositories;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("AllCORS", app => { 
        app.WithOrigins("*")
           .AllowAnyHeader()
           .AllowAnyMethod();   
    });
});

var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(mySQLConfiguration);


//Test in production
//builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));

builder.Services.AddScoped<IIngenieroRepository, IngenieroRepository>();
builder.Services.AddScoped<IControlAsistenciaRepository, ControlAsistenciaRepository>();

//builder.WebHost.UseKestrel();
//builder.WebHost.UseIIS();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllCORS");

app.UseAuthorization();

app.MapControllers();

app.Run();
 