using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PhotoContests.Entities;
using PhotoContests.Hubs;
using PhotoContests.Manager;
using PhotoContests.Repo;
using PhotoContests.Configurations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors();
// Add services to the container.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                            },
                            Scheme = "oauth2",
                            Name="Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PhotoContestsContext>(
     options =>
     {
         options.UseSqlServer(connectionString);
     });

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<PhotoContestsContext>();


builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("AuthScheme", options =>
                {
                    options.SaveToken = true;
                    var secret = builder.Configuration.GetSection("Jwt").GetSection("SecretKey").Get<String>();//luam cheia secreta
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminUser", policy => policy.RequireRole("AdminUser")
     .RequireAuthenticatedUser().AddAuthenticationSchemes("AuthScheme").Build());
    opt.AddPolicy("PhotographerUser", policy => policy.RequireRole("PhotographerUser")
     .RequireAuthenticatedUser().AddAuthenticationSchemes("AuthScheme").Build());
    opt.AddPolicy("JurorUser", policy => policy.RequireRole("JurorUser")
     .RequireAuthenticatedUser().AddAuthenticationSchemes("AuthScheme").Build());
});
builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddTransient<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddTransient<ITokenManager, TokenManager>();

builder.Services.AddTransient<IAssignementRepo, AssignementRepo>();
builder.Services.AddTransient<IAssignementManager, AssignementManager>();

builder.Services.AddTransient<ICompetitionRepo, CompetitionRepo>();
builder.Services.AddTransient<ICompetitionManager, CompetitionManager>();

builder.Services.AddTransient<IJurorRepo, JurorRepo>();
builder.Services.AddTransient<IJurorManager, JurorManager>();

builder.Services.AddTransient<INationalityRepo, NationalityRepo>();
builder.Services.AddTransient<INationalityManager, NationalityManager>();

builder.Services.AddTransient<IParticipationRepo, ParticipationRepo>();
builder.Services.AddTransient<IParticipationManager, ParticipationManager>();

builder.Services.AddTransient<IPhotoRepo, PhotoRepo>();
builder.Services.AddTransient<IPhotoManager, PhotoManager>();

builder.Services.AddTransient<IPhotographerRepo, PhotographerRepo>();
builder.Services.AddTransient<IPhotographerManager, PhotographerManager>();

builder.Services.AddTransient<IReviewRepo, ReviewRepo>();
builder.Services.AddTransient<IReviewManager, ReviewManager>();

builder.Services.AddTransient<ISectionRepo, SectionRepo>();
builder.Services.AddTransient<ISectionManager, SectionManager>();

builder.Services.AddTransient<ITypeRepo, TypeRepo>();
builder.Services.AddTransient<ITypeManager, TypeManager>();

builder.Services.AddTransient<IVideoRepo, VideoRepo>();
builder.Services.AddTransient<IVideoManager, VideoManager>();

builder.Services.AddTransient<IVotingRepo, VotingRepo>();
builder.Services.AddTransient<IVotingManager, VotingManager>();

builder.Services.AddTransient<IReportRepo, ReportRepo>();
builder.Services.AddTransient<IReportManager, ReportManager>();

builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<IUserMyManager, UserMyManager>();

builder.Services.AddTransient<IJurorRepo, JurorRepo>();
builder.Services.AddTransient<IJurorManager, JurorManager>();

builder.Services.AddTransient<IChatRepo, ChatRepo>();
builder.Services.AddTransient<IChatManager, ChatManager>();

builder.Services.AddTransient<INotificationRepo, NotificationRepo>();
builder.Services.AddTransient<INotificationManager, NotificationManager>();

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hubs/chat");

app.Run();
