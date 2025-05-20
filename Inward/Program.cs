
using Inward.Repository.Abstraction;
using Inward.Repository;
using Inward.Services.Abstraction;
using Inward.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GCM.Services.Abstraction;
using GCM.Services;
using GCM.Repository.Abstraction;
using GCM.Repository;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpContextAccessor();

// Add session services
builder.Services.AddDistributedMemoryCache(); // This is required to store session in memory
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".YourApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as per your requirements
});


builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

builder.Services.Configure<FormOptions>(options =>
{
	options.ValueLengthLimit = int.MaxValue;
	options.MultipartBodyLengthLimit = long.MaxValue; // For file uploads
	options.MemoryBufferThreshold = int.MaxValue;
});

// Register your services here
//builder.Services.AddTransient<SessionManager>(); // Example registration



builder.Services.AddControllersWithViews()
         .AddDataAnnotationsLocalization();
{
    var services = builder.Services;
    services.AddHttpContextAccessor();
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/LogOut";
    });
    services.AddHttpClient();


    services.AddScoped<ILoginService, LoginService>();
    services.AddScoped<ILoginRepo, LoginRepo>();

    services.AddScoped<IFinancialYearTermWiseFeeService, FinancialYearTermWiseFeeService>();
    services.AddScoped<IFinancialYearTermWiseFeeRepo, FinancialYearTermWiseFeeRepo>();

    services.AddScoped<IStudentService, StudentService>();
    services.AddScoped<IStudent, StudentRepo>();

    services.AddScoped<IStudentFeeCollectionService, StudentFeeCollectionService>();
    services.AddScoped<IStudentFeeCollectionRepo, StudentFeeCollectionRepo>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        // app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthorization();
    app.UseAuthentication();
    app.UseSession(); // This adds session middleware

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Login}/{id?}");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.Run();
}