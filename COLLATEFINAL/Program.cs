using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using COLLATEFINAL.Controllers;
using COLLATEFINAL.ViewModels;
using COLLATEFINAL.Models;
using COLLATEFINAL.Data;
using COLLATEFINAL.Helpers;
using COLLATEFINAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserClaimsPrincipalFactory<AppIdentityUser>, AppIdentityUserClaimsPrincipalFactory>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
}).AddMvcOptions(options =>
{
    options.FormatterMappings.SetMediaTypeMappingForFormat("pdf", "application/pdf");
});
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["App:GoogleClientId"];
        options.ClientSecret = builder.Configuration["App:GoogleClientSecret"];
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministratorPolicy",
        policy => policy.RequireClaim("Administrator", "true"));
    options.AddPolicy("EventsPolicy",
        policy => policy.RequireClaim("Events", "true"));
    options.AddPolicy("SoftwarePolicy",
        policy => policy.RequireClaim("Software Projects", "true"));
    options.AddPolicy("InstructionalPolicy",
        policy => policy.RequireClaim("Instructional Materials", "true"));
    options.AddPolicy("ResearchPolicy",
        policy => policy.RequireClaim("Research Papers", "true"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
