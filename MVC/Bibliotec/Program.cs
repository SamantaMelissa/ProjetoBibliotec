using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicionando autenticação por cookie
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie((options) => 
    { options.LoginPath = "/Login";});

// Add services Jwt Bearer : forma de autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = "JwtBearer";
    options.DefaultAuthenticateScheme = "JwtBearer";
})

//define os parâmetros de validação do token
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //valida quem está solicitando
        ValidateIssuer = true,

        //valida quem está recebendo
        ValidateAudience = true,

        //define se o tempo de expiração sera validado
        ValidateLifetime = true,

        //forma de criptografia e ainda valida a chave de autenticação
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("bibliotec-chave-autenticacao-dev")),

        //valida o tempo de expiração do token
        ClockSkew = TimeSpan.FromHours(8766),

        //nome do issuer, de onde está vindo
        ValidIssuer = "Bibliotec",

        //nome do audience, para onde está indo 
        ValidAudience = "Bibliotec"
    };
});

//session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
