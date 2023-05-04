using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// mvc uygulamalar�nda login bilgileri cookie ile saklan�r
// defa
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
  opt.Cookie.HttpOnly = false; // secure sadece https kanal�ndan aktif olcak.
  opt.Cookie.SameSite = SameSiteMode.Strict;
  //opt.Cookie.Domain = "www.a.com";
  opt.ExpireTimeSpan = TimeSpan.FromDays(30);
  opt.LoginPath = "/login";
  opt.LogoutPath = "/logout";
  opt.AccessDeniedPath = "/unauthorized";
  opt.SlidingExpiration = true; // 20 dk da bir oturumu artt�r.
});

builder.Services.AddAuthorization(policy =>
{
  policy.AddPolicy("UserDeletePolicy", options =>
  {
    options.RequireAuthenticatedUser(); // kimlik do�rulamas� gerekiyor
    options.RequireClaim("User", "Delete"); // User ClaimType Delete Value'suna sadece izin ver.
    options.RequireRole("Manager"); // Kullan�c� Manager Rol�ne sahip olmal�d�r.
  });
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
app.UseAuthentication(); // kimlik do�rulamas� middleware aktif hale getiririz.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
