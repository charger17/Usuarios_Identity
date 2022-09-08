using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Usuarios_identity.Datos;

var builder = WebApplication.CreateBuilder(args);

//configuramos la conexión a sql server
builder.Services.AddDbContext<ApplicationDbContext>(
    opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"))
);

//Agregamos el servicio Identity a la aplicación
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Esta linea es para la Url de retorno al acceder 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Cuentas/Index");

});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Estas son opciones de configuracion del identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5; //Minimo de caracteres
    options.Password.RequireLowercase = false; //REquiere caracteres en minuscula
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Cuentas}/{action=Index}/{id?}");

app.Run();
