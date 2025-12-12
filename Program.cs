using CollegeManagement.Configurations;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var corName = "SincerelyMyCors";
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    //jwt auth for swagger
    options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the bearer scheme. Enter Beare [space] and add your tokenn in the text input. Exmape: Bear sewifoironfoviov",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    } ,
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<ISmtpEmailService, SMTPMailServices>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>)); //registration of generic type
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient(typeof(IUploadExcel<>), typeof(UploadExcel<>));

builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookConn")));

//named policy for CORS - to allow use of multiple cors policy using AddPolicy method
builder.Services.AddCors(options =>
{
    //options.AddDefaultPolicy(options =>
    //{  // default policy to allow all origins
    //    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});
    //options.AddPolicy("AllowAll", options =>
    //{  // default policy to allow all origins
    //    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});
    options.AddPolicy("AllowOnlyLocalhost", options =>
    {  // allow specific origins
        options.WithOrigins("https://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
    //options.AddPolicy("AllowOnlyGoogle", options =>
    //{  // allow specific origins
    //    options.WithOrigins("https://google.com", "https://gmail.com", "https://drive.google.com").AllowAnyHeader().AllowAnyMethod();
    //});
    //options.AddPolicy("AllowOnlyMicrosoft", options =>
    //{  // allow specific origins
    //    options.WithOrigins("https://microsoft.com", "https://outlook.com", "https://onedrive.com").AllowAnyHeader().AllowAnyMethod();
    //});
});

//Jwt Authentication configuration
var signInSecretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretKey"));
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false; // set to false if having http issues // set to true in production
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(signInSecretKey),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowOnlyLocalhost");
//app.UseCors("AllowAll"); // if you are using default policy, don't give a name here
app.UseAuthentication();
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("api/testingendpoint", context => context.Response.WriteAsync("Test Response")).RequireCors("AllowOnlyLocalhost"); // example of using named policy
//    endpoints.MapControllers().RequireCors("AllowAll"); // this will map all controllers
//    endpoints.MapGet("api/testresponse2", context => context.Response.WriteAsync("Response2")); // example of using named policy
//});
app.MapControllers();

app.Run();
