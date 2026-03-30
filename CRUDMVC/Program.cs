using CRUDMVC.Data;
using CRUDMVC.Hubs;
using CRUDMVC.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var jwtkey = Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_JWT_KEY_1234567890_ABCDEF_0987654321"
);

var google = builder.Configuration.GetSection("Authentication:Google");

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = google["ClientId"];
        options.ClientSecret = google["ClientSecret"];
    })
    .AddJwtBearer("Jwt", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtkey)
        };
    });

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

//builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();


builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ILessonService, LessonService>();

builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
builder.Services.AddScoped<IProgressService, ProgressService>();

builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();





var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ChatHub>("/hubs/chat");

using (var scope = app.Services.CreateScope())
{
    var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
    await roleService.CreateRolesAsync();
}

app.Run();
