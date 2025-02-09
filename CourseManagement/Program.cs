using CourseManagement.Application;
using CourseManagement.Application.Interfaces;
using CourseManagement.Application.Services;
using CourseManagement.Infrastructure;
using CourseManagement.Infrastructure.DbModel;
using CourseManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Database Connection
var environment = builder.Environment.EnvironmentName;

// Define connection string directly (could also pull from configuration)
string dbCon = builder.Configuration.GetConnectionString("Local");

// Register the DbContext with the connection string
builder.Services.AddDbContext<CourseManagementDbContext>(options =>
    options.UseSqlServer(dbCon, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("CourseManagement.Infrastructure"); // Ensure migrations are in the correct assembly
        sqlOptions.CommandTimeout(90);                                  // Set command timeout
        sqlOptions.MinBatchSize(1);                                     // Set minimum batch size
        sqlOptions.MaxBatchSize(40);                                    // Set maximum batch size
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);  // Configure query splitting
    })
);
#endregion

#region Redis Connection
var redisSettings = builder.Configuration.GetSection("RedisCacheSettings");
if (redisSettings.GetValue<bool>("Enabled"))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(redisSettings["ConnectionString"])
    );
    builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
}
#endregion

//#region Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                ValidAudience = builder.Configuration["Jwt:Audience"],
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//            };
//        });
//#endregion

#region Service Register
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<IDepartment, DepartmentRepository>();
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
