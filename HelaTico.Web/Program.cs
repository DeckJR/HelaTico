using System.Text;
using HelaTico.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using HelaTico.Web.Middleware;
using HelaTico.Infraestructure.Repository.Interfaces;
using HelaTico.Infraestructure.Repository.Implementations;
using HelaTico.Application.Services.Interfaces;
using HelaTico.Application.Services.Implementations;
using HelaTico.Application.Profiles;
using Hangfire;
using HelaTico.Application.Jobs.Interfaces;
using HelaTico.Web.Jobs;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using HelaTico.Web.Resources.Views.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using HelaTico.Application.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "es", "en" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

//Configurar D.I. //Repository 
builder.Services.AddTransient<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddTransient<IRepositoryCombo, RepositoryCombo>();
builder.Services.AddTransient<IRepositoryMenu, RepositoryMenu>();
builder.Services.AddScoped<IRepositoryPreparacion, RepositoryPreparacion>();
builder.Services.AddTransient<IRepositoryCategoria, RepositoryCategoria>();
builder.Services.AddTransient<IRepositoryIngrediente, RepositoryIngrediente>();
builder.Services.AddScoped<IRepositoryEstacion, RepositoryEstacion>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryPedido, RepositoryPedido>();
builder.Services.AddTransient<IRepositoryTipoEntrega,RepositoryTipoEntrega>();
builder.Services.AddTransient<IRepositoryOrden,RepositoryOrden>();

//Services 
builder.Services.AddTransient<IServiceProducto, ServiceProducto>();
builder.Services.AddTransient<IServiceCombo, ServiceCombo>();
builder.Services.AddTransient<IServiceMenu, ServiceMenu>();
builder.Services.AddScoped<IServicePreparacion, ServicePreparacion>();
builder.Services.AddTransient<IServiceCategoria, ServiceCategoria>();
builder.Services.AddTransient<IServiceIngrediente, ServiceIngrediente>();
builder.Services.AddScoped<IServiceEstacion, ServiceEstacion>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServicePedido, ServicePedido>();
builder.Services.AddTransient<IServiceCarrito, ServiceCarrito>();
builder.Services.AddTransient<IServiceTipoEntrega,ServiceTipoEntrega>();
builder.Services.AddTransient<IServiceOrden,ServiceOrden>();


//Configurar Automapper 
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductoProfile>();
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<IngredienteProfile>();
    config.AddProfile<ComboProfile>();
    config.AddProfile<MenuProfile>();
    config.AddProfile<PreparacionProfile>();
    config.AddProfile<UsuarioProfile>();
});


// Configuar Conexión a la Base de Datos SQL 
builder.Services.AddDbContext<HelaTicoContext>(options => {
    // it read appsettings.json file 
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});

//***********************
//Configuracion Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
                    // Limitar la informacion de depuracion
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    // Log LogEventLevel.Verbose muestra mucha informacion, pero no es necesaria solo para el proceso de depuracion
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);
//***************************

// Registrar Hangfire con SQL Server
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServerDataBase")));

builder.Services.AddHangfireServer();

// Registrar el Job
builder.Services.AddScoped<IMenuStatusJob, MenuStatusJob>();

builder.Services.Configure<AppConfig>(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/Forbidden";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Carrito de compras en sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.IsEssential = true;
});

//Activar la memoria cache para poder tener una persistencia del tipo del cambio del dolar
builder.Services.AddMemoryCache();

//repositorio del tipo del cambio
builder.Services.AddHttpClient<IRepositoryTipoCambio, RepositoryTipoCambio>();

//servicio del tipo del cambio
builder.Services.AddScoped<IServiceTipoCambio, ServiceTipoCambio>();

var app = builder.Build();


// Activar el Dashboard de Hangfire
app.UseHangfireDashboard("/hangfire");

// Registrar el Recurring Job — se ejecuta cada minuto
RecurringJob.AddOrUpdate<IMenuStatusJob>(
    "actualizar-estado-menus",
    job => job.EjecutarAsync(),
    Cron.Minutely());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware 
    app.UseMiddleware<ErrorHandlingMiddleware>();
}


//Activar soporte a la solicitud de registro con SERILOG 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

//Empieza el traductor
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Activar Antiforgery  
app.UseAntiforgery();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();