using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using SGD.Data;
using SGD.Filters;
using SGD.Services.API;
using SGD.Services.Arquivo;
using SGD.Services.Autenticacao;
using SGD.Services.Barcode;
using SGD.Services.Fluxo;
using SGD.Services.Home;
using SGD.Services.Index;
using SGD.Services.Lote;
using SGD.Services.Preparo;
using SGD.Services.Projeto;
using SGD.Services.Scan;
using SGD.Services.SelecionarLote;
using SGD.Services.Sessao;
using SGD.Services.TipoDocumento;
using SGD.Services.Usuario;
using SGD.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessaoExpiradaFilter>();
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Home/Login/");
        options.AccessDeniedPath = new PathString("/Home/Index/");
    });

builder.Services.AddDbContext<DataDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    }).EnableSensitiveDataLogging();
});

builder.Services.AddHttpClient("apiClient",client => {
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api",""));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAutenticacaoInterface, AutenticacaoService>();
builder.Services.AddScoped<IHomeInterface, HomeService>();
builder.Services.AddScoped<ISessao, Sessao>();
builder.Services.AddScoped<IUsuarioInterface, UsuarioService>();
builder.Services.AddScoped<IProjetoInterface, ProjetoService>();
builder.Services.AddScoped<ILoteInterface, LoteService>();
builder.Services.AddScoped<IFluxoInterface, FluxoService>();
builder.Services.AddScoped<IPreparoInterface, PreparoService>();
builder.Services.AddScoped<IArquivoInterface, ArquivoService>();
builder.Services.AddScoped<ISelecionalote, SelecionarLoteService>();
builder.Services.AddScoped<ITipoDocumentoInterface, TipoDocumentoService>();
builder.Services.AddScoped<IApiInterface, ApiService>();
builder.Services.AddScoped<LoteSelecionado>();
builder.Services.AddScoped<IIndexInterface,IndexService>();
builder.Services.AddScoped<IBarcodeServiceInterface,BarcodeService>();
//builder.Services.AddHostedService<LoteWorker>();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024; // 2 GB
    options.ValueCountLimit = 10000; // permite até 10 mil campos
    options.MultipartHeadersCountLimit = 10000; // aumenta limite de headers multipart
});


builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Gestao/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 2L * 1024 * 1024 * 1024; // 2 GB
});


builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.IdleTimeout = TimeSpan.FromMinutes(40); // Timeout adicionado
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Sessão ativada aqui
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();
app.Run();
